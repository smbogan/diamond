using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewNumber : IViewField
    {
        string Formula { get; set; }

        public ViewNumber(string formula)
        {
            Formula = formula;
        }

        public bool ProvidesEntry
        {
            get
            {
                return true;
            }
        }

        public ViewTypes ViewType
        {
            get
            {
                return ViewTypes.Number;
            }
        }

        public Formula GetEntry()
        {
            return new Formula(Formula);
        }

        public IEnumerable<string> GetDependencies()
        {
            yield break;
        }
    }
}
