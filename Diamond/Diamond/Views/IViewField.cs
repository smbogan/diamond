using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Views
{
    public interface IViewField
    {
        ViewTypes ViewType { get; }
        bool ProvidesEntry { get; }
        Formula GetEntry();
        IEnumerable<string> GetDependencies();
    }
}
