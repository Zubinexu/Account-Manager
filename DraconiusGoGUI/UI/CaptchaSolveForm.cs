﻿using System;
using System.Windows.Forms;

namespace DraconiusGoGUI.UI
{
    public partial class CaptchaSolveForm : Form
    {
        public CaptchaSolveForm(string url)
        {
            captchaUrl = url;
            InitializeComponent();
        }

        private string captchaUrl = "";

        private void CaptchaSolveForm_Load(object sender, EventArgs e)
        {
            //this.webBrowser1.Navigate(captchaUrl);
            var web = new WebBrowser()
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(web);
            web.Navigate(captchaUrl);
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }
    }
}