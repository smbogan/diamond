using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewHeading : IViewField
    {
        public string Heading { get; private set; }

        public ViewHeading(string heading)
        {
            Heading = heading;
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
                return ViewTypes.Heading;
            }
        }

        public Formula GetEntry()
        {
            return new Formula('"' + Heading + '"');
        }
    }
}
