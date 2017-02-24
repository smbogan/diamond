using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Formulas
{
    public class MissingVariables : IEnumerable<string>
    {
        private List<string> missingVariableNames { get; set; } = new List<string>();

        public MissingVariables(string variableName)
        {
            missingVariableNames.Add(variableName);
        }

        public MissingVariables(IEnumerable<string> a, IEnumerable<string> b)
        {
            missingVariableNames.AddRange(a);
            missingVariableNames.AddRange(b);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)missingVariableNames).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<string>)missingVariableNames).GetEnumerator();
        }

        public override string ToString()
        {
            return "Missing: " + string.Join(", ", this);
        }
    }
}
