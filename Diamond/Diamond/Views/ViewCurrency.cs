using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewCurrency : IViewField
    {
        private string Formula { get; set; }

        public ViewCurrency(string formula)
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
                return ViewTypes.Currency;
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
