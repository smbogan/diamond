using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class CurrencyTemplate
    {

        ViewField Field { get; set; }

        public CurrencyTemplate(ViewField field)
        {
            Field = field;
        }
    }
}
