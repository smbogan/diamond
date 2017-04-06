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

        string Path { get; set; }

        public TableLinkTemplate(Controller controller, string path, ViewField viewField)
        {
            Controller = controller;
            Field = viewField;
            Path = path;
        }

        public bool PathExists()
        {
            return Controller.Cache.Exists(new ResourceIdentifier(Path, Field.ToString()));
        }
    }
}
