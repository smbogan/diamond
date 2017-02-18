using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewMethodSource
    {
        public ViewMethodSource()
        {

        }

        public IViewField Link(string link)
        {
            return new ViewLink(link);
        }

        public IViewField Options(params string[] options)
        {
            return new ViewOptions(options);
        }

        public IViewField Text()
        {
            return new ViewText();
        }

        public IViewField Currency()
        {
            return new ViewCurrency();
        }

        public IViewField Number()
        {
            return new ViewNumber();
        }
    }
}
