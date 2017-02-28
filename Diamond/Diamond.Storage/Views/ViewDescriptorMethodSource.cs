using Diamond.Storage.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage.Views
{
    public class ViewDescriptorMethodSource
    {
        public ViewDescriptorMethodSource()
        {

        }

        public IViewField Heading(Value heading)
        {
            return new ViewHeading(heading.StringValue);
        }

        public IViewField Link(Value link)
        {
            return new ViewLink(link.StringValue);
        }

        public IViewField InputOptions(params Value[] options)
        {
            return new ViewInputOptions(options.Select(o => o.StringValue).ToArray());
        }

        public IViewField InputText()
        {
            return new ViewInputText();
        }

        public IViewField Text()
        {
            return new ViewText();
        }

        public IViewField Currency(Value formula)
        {
            return new ViewCurrency(formula.StringValue);
        }

        public IViewField Number(Value formula)
        {
            return new ViewNumber(formula.StringValue);
        }
    }
}
