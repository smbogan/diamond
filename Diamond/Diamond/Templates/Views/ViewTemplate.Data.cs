using Diamond.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class ViewTemplate
    {
        TemplatedView TemplatedView { get; set; }

        string Path { get; set; }

        string BasePath { get; set; }

        Controller Controller { get; set; }

        public ViewTemplate(Controller controller, TemplatedView templatedView, string path)
        {
            Controller = controller;
            Path = path;
            TemplatedView = templatedView;

            BasePath = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
        }
    }
}
