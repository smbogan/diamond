using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    class SchemeHandlerFactory : ISchemeHandlerFactory
    {
        public Func<Controller> Controller { get; set; }

        public SchemeHandlerFactory(Func<Controller> controller)
        {
            Controller = controller;
        }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new ResourceHandler(request, Controller());
        }
    }
}
