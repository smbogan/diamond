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
    public class CompilerFactoryCache
    {
        IDictionary<string, Value> Variables { get; set; }

        Dictionary<string, Func<object>> Cache { get; set; } = new Dictionary<string, Func<object>>();

        object MethodSource { get; set; }

        public CompilerFactoryCache(IDictionary<string, Value> variables, object methodSource)
        {
            Variables = variables;
            MethodSource = methodSource;
        }

        public T Run<T>(string formula) where T : class
        {
            Func<object> formulaCompilation;

            if(!Cache.TryGetValue(formula, out formulaCompilation))
            {
                formulaCompilation = new FormulaCompiler(Variables, MethodSource).Compile(formula);

                Cache[formula] = formulaCompilation;
            }

            return formulaCompilation() as T;
        }
    }
}
