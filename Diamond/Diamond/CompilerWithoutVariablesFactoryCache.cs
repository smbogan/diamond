using Diamond.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class CompilerWithoutVariablesFactoryCache
    {
        Dictionary<string, Func<object>> Cache { get; set; } = new Dictionary<string, Func<object>>();

        object MethodSource { get; set; }

        public CompilerWithoutVariablesFactoryCache(object methodSource)
        {
            MethodSource = methodSource;
        }

        public T Run<T>(string formula) where T : class
        {
            Func<object> formulaCompilation;

            if (!Cache.TryGetValue(formula, out formulaCompilation))
            {
                formulaCompilation = new FormulaCompilerWithoutVariables(MethodSource).Compile(formula);

                Cache[formula] = formulaCompilation;
            }

            return formulaCompilation() as T;
        }
    }
}
