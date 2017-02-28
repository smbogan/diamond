using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class ViewMethodSource
    {
        Controller Controller { get; set; }

        public ViewMethodSource(Controller controller)
        {
            Controller = controller;
        }
    }
}
