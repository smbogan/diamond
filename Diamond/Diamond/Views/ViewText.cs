using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewText : IViewField
    {
        public ViewText()
        {

        }

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
                return ViewTypes.Text;
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
