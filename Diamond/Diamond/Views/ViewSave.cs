using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewSave : IViewField
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
                return ViewTypes.Save;
            }
        }

        public IEnumerable<string> GetDependencies()
        {
            yield break;
        }

        public Formula GetEntry()
        {
            return null;
        }
    }
}
