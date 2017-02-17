﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewEntry
    {
        public string Name { get; private set; }

        public string Formula { get; private set; }

        public IViewField GetViewField()
        {
            var compiler = new Formulas.FormulaCompiler(new Dictionary<string, object>()
            {

            }, new ViewMethodSource());

            return compiler.Compile(Formula)() as IViewField;
        }

        public ViewEntry(string name, string formula)
        {
            Name = name;
            Formula = formula;
        }
    }
}
