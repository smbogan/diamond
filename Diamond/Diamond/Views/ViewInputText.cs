using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewInputText : IViewField
    {
        public bool ProvidesEntry
        {
            get
            {
                return false;
            }
        }

        public ViewTypes ViewType
        {
            get
            {
                return ViewTypes.InputText;
            }
        }

        public Formula GetEntry()
        {
            throw new InvalidOperationException();
        }

        public IEnumerable<string> GetDependencies()
        {
            yield break;
        }
    }
}
