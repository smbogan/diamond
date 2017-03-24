using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class Formula
    {
        public string Content { get; set; }


        public override string ToString()
        {
            return Content;
        }

        public Formula(string content)
        {
            Content = content;
        }

        public Formula()
            : this("0")
        {

        }
    }
}
