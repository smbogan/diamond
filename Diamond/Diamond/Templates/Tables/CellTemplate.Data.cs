using Diamond.Storage;
using Diamond.Storage.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Tables
{
    public partial class CellTemplate
    {
        Controller Controller { get; set; }

        Table Table { get; set; }

        Cell Cell { get; set; }

        int Row { get; set; }

        int Column { get; set; }

        public CellTemplate(Controller controller, Table table, Cell cell, int row, int col)
        {
            Controller = controller;
            Table = table;
            Cell = cell;
            Row = row;
            Column = col;
        }

        private string RunFormula(string formula)
        {
            object result = new FormulaCompiler(new Dictionary<string, object>(), new TableFormulaMethodSource(Controller, Table)).Compile(formula)();

            return result.ToString();
        }
    }
}
