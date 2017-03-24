using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Formulas
{
    public class CompileError
    {

        List<string> errors { get; set; } = new List<string>();

        public IEnumerable<string> Errors
        {
            get
            {
                foreach(var e in errors)
                {
                    yield return e;
                }
            }
        }

        public CompileError(IEnumerable<string> errors)
        {
            this.errors.AddRange(errors);
        }

        public static CompileError operator+(CompileError a, CompileError b)
        {
            return new CompileError(a.Errors.Union(b.Errors));
        }

        public override string ToString()
        {
            return "<Formula Error>";
        }
    }
}
