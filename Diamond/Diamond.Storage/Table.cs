using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage
{
    public class Table
    {
        public int Rows
        {
            get
            {
                return data.Count;
            }
        }

        public int Columns
        {
            get
            {
                return headings.Count;
            }
        }

        private List<string> headings;

        public IEnumerable<string> Headings
        {
            get
            {
                foreach(var h in headings)
                {
                    yield return h;
                }
            }
        }

        List<List<Cell>> data = new List<List<Cell>>();
        

        public Cell this[int row, int column]
        {
            get
            {
                return data[row][column];
            }

            set
            {
                data[row][column] = value;
            }
        }

        public void InsertRow(int rowPosition)
        {
            List<Cell> row = new List<Cell>();

            for(int c = 0; c < Columns; c++)
            {
                row.Add(new Cell());
            }

            data.Insert(rowPosition, row);
        }

        public void AddRow()
        {
            InsertRow(Rows);
        }

        public void DeleteRow(int rowPosition)
        {
            data.RemoveAt(rowPosition);
        }

        public void AddColumn(string name, int columnPosition)
        {
            headings.Insert(columnPosition, name);

            foreach(var row in data)
            {
                row.Insert(columnPosition, new Cell());
            }
        }

        public void AddColumn(string name)
        {
            AddColumn(name, Columns);
        }

        public Table(Stream stream)
        {
            using (TextReader tr = new StreamReader(stream))
            {
                var reader = new CsvHelper.CsvParser(tr);

                bool first = true;

                while (true)
                {
                    var row = reader.Read();

                    if (row == null)
                        break;

                    if(first)
                    {
                        headings = new List<string>(row);
                        first = false;
                    }
                    else
                    {
                        data.Add(new List<Cell>(row.Select(r => Cell.Parse(r))));
                    }
                }
            }
        }

        public Table(IEnumerable<string> headings)
        {
            this.headings = new List<string>(headings);
        }

        public Table(params string[] headings)
            : this(headings.AsEnumerable())
        {

        }

        public void Write(Stream stream)
        {
            using (StreamWriter sr = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            {
                using (var writer = new CsvHelper.CsvWriter(sr))
                {
                    //Write header
                    foreach (var h in headings)
                    {
                        writer.WriteField(h);
                    }

                    writer.NextRecord();

                    //Write data
                    foreach (var row in data)
                    {
                        foreach (var cell in row)
                        {
                            writer.WriteField(cell.ToString());
                        }

                        writer.NextRecord();
                    }
                }
            }
        }
    }
}
