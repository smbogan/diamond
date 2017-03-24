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

        Controller Controller { get; set; }

        public FieldTemplate(Controller controller, ViewField field)
        {
            Field = field;
            Controller = controller;
        }
    }
}
