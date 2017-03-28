using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Browser
{
    public partial class DirectoryBrowser
    {
        string BasePath { get; set; }

        string Directory { get; set; }

        public DirectoryBrowser(string basePath, string directory)
        {
            BasePath = basePath;
            Directory = directory;
        }

        public bool IsRoot()
        {
            return Directory == "" || Directory == "/";
        }

        public IEnumerable<string> Directories()
        {
            return System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(BasePath, Directory.StartsWith("/") ? Directory.Substring(1) : Directory))
                .Select(d => System.IO.Path.GetFileName(d)).Where(d => d != ".git");
        }

        public IEnumerable<ResourceIdentifier> Files()
        {
            foreach(var file in System.IO.Directory.EnumerateFiles(System.IO.Path.Combine(BasePath, Directory)))
            {
                yield return new ResourceIdentifier(Directory + System.IO.Path.GetFileName(file));
            }
        }
    }
}
