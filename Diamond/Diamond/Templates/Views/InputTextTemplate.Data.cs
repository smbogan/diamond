using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class InputTextTemplate
    {
        ViewField Field { get; set; }

        public InputTextTemplate(ViewField field)
        {
            Field = field;
        }
    }
}
