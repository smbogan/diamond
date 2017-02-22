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

        private string startingUrl;

        public DiamondForm(string openUrl = "www://root/fake.table")
        {
            InitializeComponent();

            startingUrl = openUrl;
        }

        private void DiamondForm_Load(object sender, EventArgs e)
        {
            Browser = new CefSharp.WinForms.ChromiumWebBrowser(startingUrl)
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

        private void button1_Click(object sender, EventArgs e)
        {
            Browser.ShowDevTools();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new DiamondForm("www://root/");

            f.Show();
        }
    }
}
