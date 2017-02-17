using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewOptions : IViewField
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

        private string[] options;

        public ViewOptions(params string[] options)
        {
            this.options = options;
        }
    }
}
