using Diamond.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Tables
{
    public partial class TableTemplate
    {
        private Table Table { get; set; }

        private Controller Controller { get; set; }

        private string Path { get; set; }

        public TableTemplate(Controller controller, Table table, string path)
        {
            Path = path;

            Table = table;

            Controller = controller;
        }
    }
}
