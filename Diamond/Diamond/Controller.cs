using Diamond.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diamond
{
    public class Controller
    {

        public Cache Cache;

        public Controller()
        {
            Repository repo = new Repository("C:\\DiamondData", "Shaun Bogan", "smbogan@gmail.com");
            Cache = new Cache(repo);




        }

        System.Text.RegularExpressions.Regex pageTemplate =
            new System.Text.RegularExpressions.Regex("<% *?([a-zA-Z]+?) *?%>");

        public Stream ProcessTemplate(Stream stream)
        {
            Table t = new Table("A", "B", "C");
            t.AddRow();
            t[0, 0] = new Cell("Row0");
            t[0, 1] = new Cell(55m);
            t[0, 2] = new Cell("O");

            string basePage;

            using (var sr = new StreamReader(stream))
            {
                basePage = sr.ReadToEnd();
            }

            string processedPage = Regex.Replace(basePage, "<% *?([a-zA-Z]+?) *?%>", new MatchEvaluator((m) =>
            {
                string templateName = m.Groups[1].Value;

                switch(templateName)
                {
                    case "body":
                        return new Templates.Tables.TableTemplate(t).TransformText();
                }

                throw new Exception("Unknown template: " + templateName ?? "MISSING");
            }));

            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(processedPage));

            return ms;
        }
    }
}
