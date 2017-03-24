using Diamond.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewDescriptorEntry
    {
        public string Name { get; private set; }

        public string Formula { get; private set; }

        public IViewField GetViewField()
        {
            var compiler = new Formulas.FormulaCompilerWithoutVariables(/*new Dictionary<string, Value>()
            {

            }, */new ViewDescriptorMethodSource());

            return compiler.Compile(Formula)() as IViewField;
        }

        public ViewDescriptorEntry(string name, string formula)
        {
            Name = name;
            Formula = formula;
        }
    }
}
