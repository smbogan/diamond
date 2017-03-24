using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewInputOptions : IViewField
    {
        public IEnumerable<string> Options
        {
            get
            {
                foreach(var o in options)
                {
                    yield return o;
                }
            }
        }

        public ViewTypes ViewType
        {
            get
            {
                return ViewTypes.Options;
            }
        }

        public bool ProvidesEntry
        {
            get
            {
                return false;
            }
        }

        private string[] options;

        public ViewInputOptions(params string[] options)
        {
            this.options = options;
        }

        public Formula GetEntry()
        {
            throw new InvalidOperationException();
        }

        public IEnumerable<string> GetDependencies()
        {
            yield break;
        }
    }
}
