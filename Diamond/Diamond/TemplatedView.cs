using Diamond.Storage.Formulas;
using Diamond.Storage.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Diamond.Storage;

namespace Diamond
{
    public class TemplatedView : IEnumerable<ViewField>
    {
        Controller Controller { get; set; }

        ViewDescriptor Descriptor { get; set; }

        View View { get; set; }

        public Variables Variables { get; private set; }

        List<ViewField> fields = new List<ViewField>();

        Dictionary<string, Value> evaluations = new Dictionary<string, Value>();

        public TemplatedView(Controller controller, View view)
        {
            Controller = controller;

            View = view;

            var descriptor = controller.Cache.GetViewDescriptor(new Storage.ResourceIdentifier(view.Descriptor));

            Variables = new Diamond.Variables((key) =>
            {
                //var rawKey = key;

                key = new string(key.Where(c =>
                (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || (c >= '0' && c <= '9')).ToArray());

                Value result;

                if(evaluations.TryGetValue(key, out result))
                {
                    return result;
                }

                var field = fields.Where(f => (new string(f.Name.Where(c =>
                  (c >= 'a' && c <= 'z')
                  || (c >= 'A' && c <= 'Z')
                  || (c >= '0' && c <= '9')).ToArray())) == key).FirstOrDefault();

                if(field != null)
                {
                    if (field.Descriptor.ProvidesEntry)
                    {
                        var descriptorFormula = field.Descriptor.GetEntry();

                        result = new FormulaCompiler(Variables, new ViewMethodSource(Controller)).Compile(descriptorFormula.Content)() as Value;
                    }
                    else
                    {
                        if (field.Entry == null)
                            return null;

                        var cell = field.Entry.Value;

                        switch (cell.DataType)
                        {
                            case Storage.CellDataType.Decimal:
                                result = new Value(cell.GetDecimal());
                                break;
                            case Storage.CellDataType.Empty:
                                result = new Value("");
                                break;
                            case Storage.CellDataType.String:
                                result = new Value(cell.GetString());
                                break;
                            case Storage.CellDataType.Formula:
                                result = new FormulaCompiler(Variables, new ViewMethodSource(Controller)).Compile(cell.GetFormula().Content)() as Value;
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

            Dictionary<string, ViewEntry> entries = new Dictionary<string, ViewEntry>();

            foreach(var e in view)
            {
                entries[e.Name] = e;
            }

            var viewFactoryVariables = new Variables((key) =>
            {
                return new Value(new MissingVariables(key));
            });

            foreach(var d in descriptor)
            {
                var f = ViewFactory.Run(d.Formula, viewFactoryVariables);

                ViewEntry correspondingEntry;

                entries.TryGetValue(d.Name, out correspondingEntry);

                fields.Add(new ViewField(d.Name, f, correspondingEntry, Variables));
            }


        }

        public IEnumerator<ViewField> GetEnumerator()
        {
            return ((IEnumerable<ViewField>)fields).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ViewField>)fields).GetEnumerator();
        }
    }
}
