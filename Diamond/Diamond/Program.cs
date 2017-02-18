using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diamond
{
    static class Program
    {
        static Controller Controller { get; set; } = new Controller();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CefSharp.CefSettings settings = new CefSharp.CefSettings()
            {
                MultiThreadedMessageLoop = false
            };

            settings.RegisterScheme(new CefCustomScheme()
            {
                SchemeHandlerFactory = new SchemeHandlerFactory(() => Controller),
                SchemeName = "www"
            });

            Cef.EnableHighDPISupport();

            Cef.Initialize(settings);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Idle += (s, e) => CefSharp.Cef.DoMessageLoopWork();
            Application.Run(new DiamondForm());

            CefSharp.Cef.Shutdown();
        }
    }
}
