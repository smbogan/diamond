using Diamond.Formulas;
using Diamond.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class ViewField
    {
        public string Name { get; private set; }

        public string VariableName { get; private set; }

        public IViewField Descriptor { get; private set; }

        public ViewEntry Entry { get; set; }

        Variables Variables { get; set; }

        public IEnumerable<string> DependentVariables
        {
            get
            {
                foreach(var d in Descriptor.GetDependencies())
                {
                    yield return d;
                }

                if(Descriptor.ProvidesEntry)
                {
                    foreach(var x in FormulaVariableExtractor.GetVariables(Descriptor.GetEntry().Content))
                    {
                        yield return x;
                    }
                }
                else if(Entry == null || Entry.Value == null)
                {
                    yield break;
                }
                else if(Entry.Value.DataType == CellDataType.Formula)
                {
                    foreach (var x in FormulaVariableExtractor.GetVariables(Entry.Value.GetFormula().Content))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

        public ViewField(string name, IViewField descriptor, ViewEntry entry, Variables variables)
        {
            Name = name;
            VariableName = new string(name.Where(c =>
                (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || (c >= '0' && c <= '9')).ToArray());

            Descriptor = descriptor;
            Entry = entry;
            Variables = variables;
        }

        public Value GetValue()
        {
            if (!Descriptor.ProvidesEntry
                && Entry != null
                && Entry.Value.DataType != CellDataType.Formula)
            {
                if (Entry.Value.DataType == CellDataType.Empty)
                    return new Value("");

                return Entry.Value.ToValue();
            }

            var value = Variables[VariableName];

            return value;
        }

        public override string ToString()
        {
            if (!Descriptor.ProvidesEntry
                && Entry != null
                && Entry.Value.DataType != CellDataType.Formula)
            {
                if (Entry.Value.DataType == CellDataType.Empty)
                    return "";

                return Entry.Value.ToString();
            }

            var value = Variables[VariableName];

            return value.ToString();
        }
    }
}
