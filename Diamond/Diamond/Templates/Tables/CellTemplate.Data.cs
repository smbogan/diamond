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

        public CellTemplate(Controller controller, Table table, Cell cell)
        {
            Controller = controller;
            Table = table;
            Cell = cell;
        }

        private string RunFormula(string formula)
        {
            object result = new FormulaCompiler(new Dictionary<string, object>(), new TableFormulaMethodSource(Controller, Table)).Compile(formula)();

            return result.ToString();
        }
    }
}
