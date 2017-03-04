using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewTableLink : IViewField
    {
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

        private List<string> headings = new List<string>();

        private string pathFormula;

        private string valueFormula;

        public ViewTableLink(string valueFormula, string pathFormula, params string[] headings)
        {
            this.valueFormula = valueFormula;

            this.pathFormula = pathFormula;

            this.headings = new List<string>(headings);
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
                return ViewTypes.TableLink;
            }
        }

        public Formula GetPath()
        {
            return new Formula(pathFormula);
        }

        public Formula GetEntry()
        {
            return new Formula(valueFormula);
        }
    }
}
