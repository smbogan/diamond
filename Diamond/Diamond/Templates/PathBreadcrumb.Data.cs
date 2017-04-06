using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates
{
    public partial class PathBreadcrumb
    {
        public string[] UrlParts { get; set; }

        public PathBreadcrumb(string url)
        {
            UrlParts = url.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
