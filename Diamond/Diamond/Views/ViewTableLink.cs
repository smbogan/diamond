using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
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

        public ViewTableLink(string pathFormula, params string[] headings)
        {
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

        public Formula GetEntry()
        {
            return new Formula(pathFormula);
        }

        public IEnumerable<string> GetDependencies()
        {
            yield break;
        }
    }
}
