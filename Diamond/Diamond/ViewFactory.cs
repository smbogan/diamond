using Diamond.Storage;
using Diamond.Storage.Formulas;
using Diamond.Storage.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public static class ViewFactory
    {
        public static IViewField Run(string formula, IDictionary<string, Value> variables)
        {
            return new FormulaCompiler(variables, new Diamond.Storage.Views.ViewDescriptorMethodSource()).Compile(formula)() as IViewField;
        }
    }
}
