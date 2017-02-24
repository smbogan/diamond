using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public static class Html
    {
        public static string Escape(string value)
        {
            return WebUtility.HtmlEncode(value);
        }
    }
}
