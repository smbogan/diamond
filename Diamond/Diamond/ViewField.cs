using Diamond.Storage;
using Diamond.Storage.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class ViewField
    {
        public string Name { get; private set; }

        public IViewField Descriptor { get; private set; }

        public ViewEntry Entry { get; set; }

        Variables Variables { get; set; }

        public ViewField(string name, IViewField descriptor, ViewEntry entry, Variables variables)
        {
            Name = name;
            Descriptor = descriptor;
            Entry = entry;
            Variables = variables;
        }

        public override string ToString()
        {
            if (!Descriptor.ProvidesEntry
                && Entry != null
                && Entry.Value.DataType != CellDataType.Formula)
            {
                if (Entry.Value.DataType == CellDataType.Empty)
                    return "";

                return Entry.Value.ToString();
            }

            var value = Variables[Name];

            return value.ToString();
        }
    }
}
