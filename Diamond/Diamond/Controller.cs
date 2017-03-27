using Diamond.Views;
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

        Dictionary<ResourceIdentifier, TemplatedView> Templates { get; set; } = new Dictionary<ResourceIdentifier, TemplatedView>();

        public Controller()
        {
            Repository repo = new Repository("C:\\DiamondData", "Shaun Bogan", "smbogan@gmail.com");
            Cache = new Cache(repo);
        }

        private TemplatedView GetTemplate(ResourceIdentifier viewIdentifier)
        {
            TemplatedView result;
            
            if(Templates.TryGetValue(viewIdentifier, out result))
            {
                return result;
            }

            var view = Cache.GetView(viewIdentifier);

            result = new Diamond.TemplatedView(this, view);

            Templates[viewIdentifier] = result;

            return result;
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

        public string GetRenderedViewEntry(string path, string variableName)
        {
            TemplatedView templatedView = GetTemplate(new ResourceIdentifier(path));

            templatedView.ClearEvaluations();

            var field = templatedView.Where(f => f.VariableName == variableName).FirstOrDefault();

            return new FieldTemplate(this, System.IO.Path.GetDirectoryName(path), field).TransformText();

            //return Html.Escape(field.ToString());
        }

        public void CreateTable(string viewPath, string variableName, string path)
        {
            TemplatedView templatedView = GetTemplate(new Diamond.ResourceIdentifier(viewPath));

            var viewField = templatedView.Where(f => f.VariableName == variableName).First();

            var tableLink = viewField.Descriptor as Views.ViewTableLink;

            var headings = tableLink.Headings;

            var newTable = new Table(headings);

            Cache.SaveTable(new ResourceIdentifier(path), newTable);
        }

        public void CreateView(string viewPath, string variableName, string path)
        {
            TemplatedView templatedView = GetTemplate(new Diamond.ResourceIdentifier(viewPath));

            var viewField = templatedView.Where(f => f.VariableName == variableName).First();

            var viewLink = viewField.Descriptor as Views.ViewViewLink;

            var descriptor = viewLink.Descriptor;

            var newView = new View(descriptor);

            Cache.SaveView(new ResourceIdentifier(path), newView);
        }

        public void UpdateTableValue(string path, int row, int col, string value)
        {
            var table = Cache.GetTable(new ResourceIdentifier(path));

            table[row, col] = Cell.Parse(value);
        }

        public void UpdateViewEntryValue(string path, string fieldName, string value)
        {
            var templateView = GetTemplate(new ResourceIdentifier(path));

            var view = templateView.View;

            var field = view.Where(f => f.Name == fieldName).FirstOrDefault();

            if(field == null)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    view.Add(new ViewEntry(fieldName, Cell.Parse(value)));
                    templateView.UpdateFields();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    view.Remove((e) => e.Name == fieldName);
                    templateView.UpdateFields();
                }
                else
                {
                    field.Value = Cell.Parse(value);
                    templateView.UpdateFields();
                }
                
            }
        }

        public string[] GetDependents(string path, string variable)
        {
            TemplatedView templatedView = GetTemplate(new ResourceIdentifier(path));

            var deps =  templatedView.Graph.GetDependents(variable).ToArray();

            return deps;
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
                        TemplatedView templatedView = GetTemplate(identifier);

                        var template = new ViewTemplate(this, templatedView, identifier.Identifier);

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
