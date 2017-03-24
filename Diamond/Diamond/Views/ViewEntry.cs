using Diamond.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewEntry
    {
        public string Name { get; private set; }

        public Cell Value { get; set; }

        public ViewEntry(string name, Cell value)
        {
            Name = name;
            Value = value;
        }
    }
}
