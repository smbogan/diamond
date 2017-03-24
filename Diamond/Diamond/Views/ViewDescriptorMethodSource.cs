using Diamond.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public class ViewDescriptorMethodSource
    {
        public ViewDescriptorMethodSource()
        {

        }

        public IViewField Heading(string heading)
        {
            return new ViewHeading(heading);
        }

        public IViewField TableLink(string path, params string[] headings)
        {
            return new ViewTableLink(path, headings.Select(h => h).ToArray());
        }

        public IViewField ViewLink(string descriptor, string path)
        {
            return new ViewViewLink(descriptor, new Value(path));
        }

        public IViewField Link(string link)
        {
            return new ViewLink(link);
        }

        public IViewField InputOptions(params string[] options)
        {
            return new ViewInputOptions(options.Select(o => o).ToArray());
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
