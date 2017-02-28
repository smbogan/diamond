using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewLink : IViewField
    {
        public string Link { get; private set; }

        public ViewTypes ViewType
        {
            get
            {
                return ViewTypes.Link;
            }
        }

        public bool ProvidesEntry
        {
            get
            {
                return true;
            }
        }

        public ViewLink(string link)
        {
            Link = link;
        }

        public Formula GetEntry()
        {
            return new Formula('"' + Link + '"');
        }
    }
}
