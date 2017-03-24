using Diamond.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewViewLink : IViewField
    {

        public string Descriptor { get; set; }

        public string Path { get; set; }

        public bool MissingVariables { get; set; }

        public string MissingVariablesString { get; set; }

        public string[] Missings { get; set; }

        public ViewViewLink(string descriptorPath, Value viewPath)
        {
            Descriptor = descriptorPath;
            Path = viewPath.StringValue;
            MissingVariables = viewPath.TypeOfValue == Value.ValueType.MissingValue;

            if (MissingVariables)
            {
                MissingVariablesString = viewPath.ToString();
                Missings = viewPath.MissingVariables.ToArray();
            }

        }

        public IEnumerable<string> GetDependencies()
        {
            yield break;

            foreach (var m in Missings)
                yield return m;
        }

        public bool ProvidesEntry
        {
            get
            {
                return true;
            }
        }

        public ViewTypes ViewType
        {
            get
            {
                return ViewTypes.ViewLink;
            }
        }

        public Formula GetEntry()
        {
            return new Formula("\"" + Path + "\"");
        }
    }
}
