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

        public ViewLink(string link)
        {
            Link = link;
        }
    }
}
