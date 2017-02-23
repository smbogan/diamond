using Diamond.Storage;
using Diamond.Templates;
using Diamond.Templates.Tables;
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

        static Dictionary<ResourceType, Type> templates;

        static Controller()
        {
            templates = new Dictionary<ResourceType, Type>();

            foreach(var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var resourceType = PageAttribute.GetPage(type);

                if(resourceType != ResourceType.Unknown)
                {
                    templates[resourceType] = type;
                }
            }
        }

        public Controller()
        {
            Repository repo = new Repository("C:\\DiamondData", "Shaun Bogan", "smbogan@gmail.com");
            Cache = new Cache(repo);




        }

        public string GetRawTableCell(string path, int row, int col)
        {
            return Cache.GetTable(new ResourceIdentifier(path))[row, col].ToString();
        }

        public string GetRenderedTableCell(string path, int row, int col)
        {
            var table = Cache.GetTable(new ResourceIdentifier(path));

            return new CellTemplate(this, table, table[row, col], row, col).TransformText();
        }

        public void UpdateTableValue(string path, int row, int col, string value)
        {
            var table = Cache.GetTable(new ResourceIdentifier(path));

            table[row, col] = Cell.Parse(value);
        }

        System.Text.RegularExpressions.Regex pageTemplate =
            new System.Text.RegularExpressions.Regex("<% *?([a-zA-Z]+?) *?%>");

        public Stream ProcessTemplate(ResourceIdentifier identifier)
        {
            string result = null;

            var templateType = templates[identifier.ResourceType];

            switch(identifier.ResourceType)
            {
                case ResourceType.Table:
                    {
                        var constructor = templateType.GetConstructor(new Type[] { typeof(Controller), typeof(Table), typeof(string) });

                        var table = Cache.GetTable(identifier);

                        var template = constructor.Invoke(new object[] { this, table, identifier.Identifier });

                        var transformMethod = templateType.GetMethod("TransformText", new Type[] { });

                        result = transformMethod.Invoke(template, new object[] { }) as string;
                    }
                    break;
            }

            if (result == null)
                return null;


            return new MemoryStream(Encoding.UTF8.GetBytes(result));

            //Table t = new Table("A", "B", "C");
            //t.AddRow();
            //t[0, 0] = new Cell("Row0");
            //t[0, 1] = new Cell(55m);
            //t[0, 2] = new Cell("O");

            //string basePage;

            //using (var sr = new StreamReader(stream))
            //{
            //    basePage = sr.ReadToEnd();
            //}

            //string processedPage = Regex.Replace(basePage, "<% *?([a-zA-Z]+?) *?%>", new MatchEvaluator((m) =>
            //{
            //    string templateName = m.Groups[1].Value;

            //    switch(templateName)
            //    {
            //        case "body":
            //            return new Templates.Tables.TableTemplate(t).TransformText();
            //    }

            //    throw new Exception("Unknown template: " + templateName ?? "MISSING");
            //}));

            //var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(processedPage));

            //return ms;
        }
    }
}
