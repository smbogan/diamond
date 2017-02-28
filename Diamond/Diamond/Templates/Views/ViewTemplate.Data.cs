using Diamond.Storage.Views;
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

        public ViewTemplate(TemplatedView templatedView, string path)
        {
            Path = path;
            TemplatedView = templatedView;
        }
    }
}
