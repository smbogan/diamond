using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class TableLinkTemplate
    {

        ViewField Field { get; set; }

        Controller Controller { get; set; }

        public TableLinkTemplate(Controller controller, ViewField viewField)
        {
            Controller = controller;
            Field = viewField;
        }

        public bool PathExists()
        {
            return Controller.Cache.Exists(new ResourceIdentifier(Field.ToString()));
        }
    }
}
