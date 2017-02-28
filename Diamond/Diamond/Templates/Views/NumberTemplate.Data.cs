using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class NumberTemplate
    {
        ViewField Field { get; set; }

        public NumberTemplate(ViewField field)
        {
            Field = field;
        }
    }
}
