using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class HeadingTemplate
    {
        ViewField Field { get; set; }

        public HeadingTemplate(ViewField field)
        {
            Field = field;
        }
    }
}
