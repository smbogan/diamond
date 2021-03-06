﻿using Diamond.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates.Views
{
    public partial class ViewLinkTemplate
    {
        ViewField Field { get; set; }

        Controller Controller { get; set; }

        string ViewPath { get; set; }

        public ViewLinkTemplate(Controller controller, string viewPath, ViewField field)
        {
            Controller = controller;
            Field = field;
            ViewPath = viewPath;
        }

        public bool MissingVariables()
        {
            var value = Field.GetValue();

            if (value.TypeOfValue != Formulas.Value.ValueType.MissingValue)
                return false;

            return true;
        }

        public string MissingVariablesString()
        {
            return Field.GetValue().ToString();
        }

        public bool PathExists()
        {
            return Controller.Cache.Exists(new ResourceIdentifier(ViewPath, Field.ToString()));
        }
    }
}
