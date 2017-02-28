using Diamond.Storage;
using Diamond.Storage.Views;
using Diamond.Templates;
using Diamond.Templates.Tables;
using Diamond.Templates.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diamond
{
    public class Controller
    {
        public Cache Cache { get; private set; }

        public Controller()
        {
            Repository repo = new Repository("C:\\DiamondData", "Shaun Bogan", "smbogan@gmail.com");
            Cache = new Cache(repo);
        }

        public string GetRawTableCell(string path, int row, int col)
        {
            return Cache.GetTable(new ResourceIdentifier(path))[row, col].ToString();
        }

        public string GetRawViewEntry(string path, string fieldName)
        {
            var view = Cache.GetView(new ResourceIdentifier(path));

            var viewEntry = view.Where(e => e.Name == fieldName).FirstOrDefault();

            if (viewEntry == null)
                return "";

            return viewEntry.Value.ToString();
        }

        public string GetRenderedTableCell(string path, int row, int col)
        {
            var table = Cache.GetTable(new ResourceIdentifier(path));

            return new CellTemplate(this, table, table[row, col], row, col).TransformText();
        }

        public string GetRenderedViewEntry(string path, string fieldName)
        {
            var view = Cache.GetView(new ResourceIdentifier(path));

            TemplatedView templatedView = new TemplatedView(this, view);

            var field = templatedView.Where(f => f.Name == fieldName).FirstOrDefault();

            return new InputTextTemplate(field).TransformText();

            //return Html.Escape(field.ToString());
        }

        public void UpdateTableValue(string path, int row, int col, string value)
        {
            var table = Cache.GetTable(new ResourceIdentifier(path));

            table[row, col] = Cell.Parse(value);
        }

        public void UpdateViewEntryValue(string path, string fieldName, string value)
        {
            var view = Cache.GetView(new ResourceIdentifier(path));

            var field = view.Where(f => f.Name == fieldName).FirstOrDefault();

            if(field == null)
            {
                view.Add(new ViewEntry(fieldName, Cell.Parse(value)));
            }
            else
            {
                field.Value = Cell.Parse(value);
            }
        }

        System.Text.RegularExpressions.Regex pageTemplate =
            new System.Text.RegularExpressions.Regex("<% *?([a-zA-Z]+?) *?%>");

        public Stream ProcessTemplate(ResourceIdentifier identifier)
        {
            string result = null;

            switch(identifier.ResourceType)
            {
                case ResourceType.Table:
                    {
                        var table = Cache.GetTable(identifier);

                        var template = new TableTemplate(this, table, identifier.Identifier);

                        result = template.TransformText();
                    }
                    break;
                case ResourceType.View:
                    {
                        var view = Cache.GetView(identifier);

                        TemplatedView templatedView = new TemplatedView(this, view);

                        var template = new ViewTemplate(templatedView, identifier.Identifier);

                        result = template.TransformText();
                    }
                    break;
            }

            if (result == null)
                return null;


            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        public void SaveTable(string path)
        {
            Cache.SaveTable(new ResourceIdentifier(path));
        }

        public void SaveView(string path)
        {
            Cache.SaveView(new ResourceIdentifier(path));
        }

        public void AddTableRow(string path)
        {
            Table table = Cache.GetTable(new ResourceIdentifier(path));
            table.AddRow();
        }

        public void AddTableRowAbove(string path, int currentRow)
        {
            Table table = Cache.GetTable(new ResourceIdentifier(path));

            table.InsertRow(currentRow);
        }

        public void AddTableRowBelow(string path, int currentRow)
        {
            Table table = Cache.GetTable(new ResourceIdentifier(path));

            table.InsertRow(currentRow + 1);
        }

        List<Cell> copiedCells = null;

        public void CopyTableRow(string path, int currentRow)
        {
            Table table = Cache.GetTable(new ResourceIdentifier(path));

            copiedCells = new List<Cell>();

            for (int c = 0; c < table.Columns; c++)
            {
                copiedCells.Add(new Cell(table[currentRow, c]));
            }
        }

        public void PasteTableRowAbove(string path, int currentRow)
        {
            if (copiedCells == null)
                return;

            Table table = Cache.GetTable(new ResourceIdentifier(path));

            if (copiedCells.Count != table.Columns)
                return;

            table.InsertRow(currentRow);

            for(int c = 0; c < table.Columns; c++)
            {
                table[currentRow, c] = copiedCells[c];
            }
        }

        public void PasteTableRowBelow(string path, int currentRow)
        {
            if (copiedCells == null)
                return;

            Table table = Cache.GetTable(new ResourceIdentifier(path));

            if (copiedCells.Count != table.Columns)
                return;

            table.InsertRow(currentRow + 1);

            for (int c = 0; c < table.Columns; c++)
            {
                table[currentRow + 1, c] = copiedCells[c];
            }
        }

        public void DeleteTableRow(string path, int currentRow)
        {
            Table table = Cache.GetTable(new ResourceIdentifier(path));

            table.DeleteRow(currentRow);
        }
    }
}
