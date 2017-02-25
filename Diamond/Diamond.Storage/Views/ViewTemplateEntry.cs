using Diamond.Storage.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewTemplateEntry
    {
        public string Name { get; private set; }

        public string Formula { get; private set; }

        public IViewField GetViewField()
        {
            var compiler = new Formulas.FormulaCompiler(new Dictionary<string, Value>()
            {

            }, new ViewMethodSource());

            return compiler.Compile(Formula)() as IViewField;
        }

        public ViewTemplateEntry(string name, string formula)
        {
            Name = name;
            Formula = formula;
        }
    }
}
