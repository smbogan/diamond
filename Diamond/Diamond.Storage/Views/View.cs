using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Diamond.Storage.Views
{
    public class View : IEnumerable<ViewEntry>
    {
        private static Parser<string> DoubleString =
            from leading in Parse.WhiteSpace.Many()
            from startString in Parse.Char('"').Once()
            from content in Parse.AnyChar.Except(Parse.Char('"')).Many().Text()
            from endString in Parse.Char('"').Once()
            from trailing in Parse.WhiteSpace.Many()
            select string.Concat(content);

        private static Parser<ViewEntry> Entry =
            from name in DoubleString
            from x in Parse.Char(':').Once()
            from w in Parse.WhiteSpace.Many()
            from value in Parse.AnyChar.Many().End().Text()
            select new ViewEntry(name, Cell.Parse(value));

        public string Descriptor { get; private set; }

        private List<ViewEntry> Entries { get; set; } = new List<ViewEntry>();

        public void Add(ViewEntry entry)
        {
            Entries.Add(entry);
        }

        public void Remove(Predicate<ViewEntry> predicate)
        {
            Entries.RemoveAll(predicate);
        }

        public View(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                string entry = sr.ReadLine();

                //First line is the template path
                Descriptor = entry;

                entry = sr.ReadLine();

                if (entry != null)
                {
                    do
                    {
                        var viewEntry = Entry.Parse(entry);

                        Entries.Add(viewEntry);

                        entry = sr.ReadLine();
                    } while (entry != null);
                }
            }
        }

        public void Write(Stream stream)
        {
            using (var sw = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            {
                sw.WriteLine(Descriptor);

                foreach(var entry in this)
                {
                    if (entry.Value != null && entry.Value.DataType != CellDataType.Empty)
                    {
                        sw.Write(@"""{0}"": {1}", entry.Name.Replace("\"", "\"\""), entry.Value.ToString());
                    }
                }
            }
        }

        public IEnumerator<ViewEntry> GetEnumerator()
        {
            return ((IEnumerable<ViewEntry>)Entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ViewEntry>)Entries).GetEnumerator();
        }
    }
}
