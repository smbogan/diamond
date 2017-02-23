﻿using CefSharp;
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

        Controller Controller { get; set; }

        public DiamondForm(Controller controller, string openUrl = "www://root/fake.table")
        {
            Controller = controller;

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

            Browser.RegisterJsObject("controller", Controller);

            Browser.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Browser.ShowDevTools();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new DiamondForm(Controller, "www://root/");

            f.Show();
        }
    }
}
