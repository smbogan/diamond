using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diamond
{
    public partial class DiamondForm : Form
    {
        CefSharp.WinForms.ChromiumWebBrowser Browser { get; set; }

        public DiamondForm()
        {
            InitializeComponent();
        }

        private void DiamondForm_Load(object sender, EventArgs e)
        {
            Browser = new CefSharp.WinForms.ChromiumWebBrowser("www://web/index.html")
            {
                BrowserSettings = new BrowserSettings()
                {
                    WebSecurity = CefState.Disabled,
                    ApplicationCache = CefState.Disabled,
                }
            };

            Controls.Add(Browser);

            Browser.Dock = DockStyle.Fill;
        }
    }
}
