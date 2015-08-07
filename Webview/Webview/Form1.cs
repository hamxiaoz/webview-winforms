using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using RazorEngine;
using RazorEngine.Templating;
using Webview.Properties;

namespace Webview
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            _browser = new ChromiumWebBrowser(null)
            {
                Dock = DockStyle.Fill,
                MenuHandler = new NoMenuHandler() // disable menu
            };
            this.panel1.Controls.Add(_browser);
        }

        private ChromiumWebBrowser _browser;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (_browser != null)
            {
                _browser.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var data = new
            {
                ShowHeader = this.checkBox1.Checked,
                Headers = new[] {"header 1", "header 2", "header 3"},
                Columns = new[] {10,20,30}
            };
            var result = Engine.Razor.RunCompile(Resources.csv_template, "templateKey", null, data);
            _browser.LoadHtml(result, "http://example/"); // https://github.com/cefsharp/CefSharp/issues/448
        }
    }

    internal class NoMenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams p)
        {
            // Return false if you want to disable the context menu.
            return false;
        }
    }
}
