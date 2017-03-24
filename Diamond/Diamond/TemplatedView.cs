using Diamond.Formulas;
using Diamond.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Diamond
{
    public class TemplatedView : IEnumerable<ViewField>
    {
        Controller Controller { get; set; }

        ViewDescriptor Descriptor { get; set; }

        public View View { get; set; }

        public Variables Variables { get; private set; }

        List<ViewField> fields = new List<ViewField>();

        Dictionary<string, Value> evaluations = new Dictionary<string, Value>();

        public ViewGraph Graph { get; private set; }

        private CompilerWithoutVariablesFactoryCache ViewFactory { get; set; }

        private CompilerFactoryCache CellFactory { get; set; }

        public TemplatedView(Controller controller, View view)
        {
            Controller = controller;

            View = view;

            Descriptor = controller.Cache.GetViewDescriptor(new ResourceIdentifier(view.Descriptor));

            Variables = new Diamond.Variables((key) =>
            {
                //key = new string(key.Where(c =>
                //(c >= 'a' && c <= 'z')
                //|| (c >= 'A' && c <= 'Z')
                //|| (c >= '0' && c <= '9')).ToArray());

                Value result;

                if(evaluations.TryGetValue(key, out result))
                {
                    return result;
                }

                var field = fields.Where(f => f.VariableName == key).FirstOrDefault();

                if(field != null)
                {
                    if (field.Descriptor.ProvidesEntry)
                    {
                        var descriptorFormula = field.Descriptor.GetEntry();

                        //result = new FormulaCompiler(Variables, new ViewMethodSource(Controller)).Compile(descriptorFormula.Content)() as Value;
                        result = CellFactory.Run<Value>(descriptorFormula.Content);
                    }
                    else
                    {
                        if (field.Entry == null)
                            return null;

                        var cell = field.Entry.Value;

                        switch (cell.DataType)
                        {
                            case CellDataType.Decimal:
                                result = new Value(cell.GetDecimal());
                                break;
                            case CellDataType.Empty:
                                result = new Value("");
                                break;
                            case CellDataType.String:
                                result = new Value(cell.GetString());
                                break;
                            case CellDataType.Formula:
                                //result = new FormulaCompiler(Variables, new ViewMethodSource(Controller)).Compile(cell.GetFormula().Content)() as Value;
                                result = CellFactory.Run<Value>(cell.GetFormula().Content);
                                break;
                        }
                    }
                }

                if (result == null)
                {
                    return new Value(new MissingVariables(key));
                }

                evaluations[key] = result;

                return result;
            });

            CellFactory = new Diamond.CompilerFactoryCache(Variables, new ViewMethodSource(Controller));

            Dictionary<string, ViewEntry> entries = new Dictionary<string, ViewEntry>();

            foreach(var e in view)
            {
                entries[e.Name] = e;
            }

            //var viewFactoryVariables = new Variables((key) =>
            //{
            //    return new Value(new MissingVariables(key));
            //});

            ViewFactory = new Diamond.CompilerWithoutVariablesFactoryCache(new Diamond.Views.ViewDescriptorMethodSource());

            foreach(var d in Descriptor)
            {
                var f = ViewFactory.Run<IViewField>(d.Formula);

                ViewEntry correspondingEntry;

                entries.TryGetValue(d.Name, out correspondingEntry);

                fields.Add(new ViewField(d.Name, f, correspondingEntry, Variables));
            }

            Graph = new Diamond.ViewGraph(this);            
        }

        public void UpdateFields()
        {
            Dictionary<string, ViewEntry> entries = new Dictionary<string, ViewEntry>();

            foreach (var e in View)
            {
                entries[e.Name] = e;
            }

            fields.Clear();

            foreach (var d in Descriptor)
            {
                var f = ViewFactory.Run<IViewField>(d.Formula);

                ViewEntry correspondingEntry;

                entries.TryGetValue(d.Name, out correspondingEntry);

                fields.Add(new ViewField(d.Name, f, correspondingEntry, Variables));
            }

            Graph = new Diamond.ViewGraph(this);
        }

        public void RegenerateGraph()
        {
            Graph = new Diamond.ViewGraph(this);
        }

        public IEnumerator<ViewField> GetEnumerator()
        {
            return ((IEnumerable<ViewField>)fields).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ViewField>)fields).GetEnumerator();
        }

        public void ClearEvaluations()
        {
            evaluations.Clear();
        }
    }
}
