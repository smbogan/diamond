﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace Diamond.Views
{
    public class ViewDescriptor : IEnumerable<ViewDescriptorEntry>
    {


        private static Parser<string> DoubleString =
            from leading in Parse.WhiteSpace.Many()
            from startString in Parse.Char('"').Once()
            from content in Parse.AnyChar.Except(Parse.Char('"')).Many().Text()
            from endString in Parse.Char('"').Once()
            from trailing in Parse.WhiteSpace.Many()
            select string.Concat(content);

        private static Parser<ViewDescriptorEntry> Entry =
            from name in DoubleString
            from x in Parse.Char(':').Once()
            from w in Parse.WhiteSpace.Many()
            from formula in Parse.AnyChar.Many().End().Text()
            select new ViewDescriptorEntry(name, formula);

        private List<ViewDescriptorEntry> Entries { get; set; } = new List<ViewDescriptorEntry>();

        public ViewDescriptor(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                string entry = sr.ReadLine();

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

        public IEnumerator<ViewDescriptorEntry> GetEnumerator()
        {
            foreach(var ve in Entries)
            {
                yield return ve;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var ve in Entries)
            {
                yield return ve;
            }
        }
    }
}
