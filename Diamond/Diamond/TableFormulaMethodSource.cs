using Diamond.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class TableFormulaMethodSource
    {
        private Controller Controller { get; set; }

        private Table Table { get; set; }

        public TableFormulaMethodSource(Controller controller, Table table)
        {
            Table = table;
            Controller = controller;
        }


    }
}
