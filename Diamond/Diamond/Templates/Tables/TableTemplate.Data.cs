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

        public TableTemplate(Table table)
        {
            Table = table;

            
        }
    }
}
