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

        string Path { get; set; }

        public FieldTemplate(Controller controller, string viewPath, ViewField field)
        {
            Field = field;
            Controller = controller;
            Path = viewPath;
        }
    }
}
