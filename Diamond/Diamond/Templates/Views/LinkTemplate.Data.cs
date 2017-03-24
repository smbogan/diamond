using Diamond.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class LinkTemplate
    {
        ViewField Field { get; set; }

        public LinkTemplate(ViewField field)
        {
            Field = field;
        }
    }
}
