using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class FieldTemplate
    {

        ViewField Field { get; set; }

        public FieldTemplate(ViewField field)
        {
            Field = field;
        }
    }
}
