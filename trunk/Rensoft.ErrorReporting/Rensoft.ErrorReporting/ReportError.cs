using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.ErrorReporting.ErrorReportingWebService;
using System.Xml;
using System.Diagnostics;
using System.Net;

namespace Rensoft.ErrorReporting
{
    public partial class ReportError : Form
    {
        private ReportingService service;
        private string clientCode;
        private string exceptionType;
        private string exceptionDetails;
        private string executablePath;

        protected ReportError()
        {
            InitializeComponent();
        }

        public ReportError(string detailsFilePath)
        {
            InitializeComponent();

            this.datPathTextBox.Text = detailsFilePath;

            XmlDocument document = new XmlDocument();
            document.Load(detailsFilePath);

            foreach (XmlElement element in document.DocumentElement)
            {
                switch (element.Name)
                {
                    case "serviceUrl":
                        this.service = new ReportingService();
                        this.service.Url = element.InnerText;
                        this.service.Timeout = 5000;
                        break;

                    case "proxy":

                        string host = null;
                        int port = 0;
                        bool useDefaultCredentials = false;
                        string user = null;
                        string pass = null;

                        foreach (XmlAttribute attribute in element.Attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "host":
                                    host = attribute.Value;
                                    break;

                                case "port":
                                    port = int.Parse(attribute.Value);
                                    break;

                                case "udc":
                                    useDefaultCredentials = bool.Parse(attribute.Value);
                                    break;

                                case "user":
                                    user = attribute.Value;
                                    break;

                                case "pass":
                                    pass = attribute.Value;
                                    break;
                            }
                        }

                        WebProxy proxy = new WebProxy(host, port);
                        proxy.UseDefaultCredentials = useDefaultCredentials;
                        if (!proxy.UseDefaultCredentials)
                        {
                            proxy.Credentials = new NetworkCredential(user, pass);
                        }
                        this.service.Proxy = proxy;

                        break;

                    case "useSystemProxy":
                        bool useSystemProxy = bool.Parse(element.InnerText);
                        if (useSystemProxy)
                        {
                            // This proxy setup works and sends the username, do not change.
                            this.service.Proxy = WebRequest.GetSystemWebProxy();
                            this.service.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        }
                        break;

                    case "clientCode":
                        this.clientCode = element.InnerText;
                        break;

                    case "exceptionType":
                        this.exceptionType = element.InnerText;
                        break;

                    case "exceptionDetails":
                        this.exceptionDetails = element.InnerText;
                        break;

                    case "executablePath":
                        this.executablePath = element.InnerText;
                        break;
                }
            }

            service.ReportCompleted += new ReportCompletedEventHandler(service_ReportCompleted);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Show();
            Focus();
            Activate();
            BringToFront();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            commentsTextBox.Enabled = false;
            _acceptButton.Enabled = false;

            service.ReportAsync(
                clientCode, commentsTextBox.Text, exceptionType,
                exceptionDetails, Environment.UserName, Environment.MachineName);
        }

        private void service_ReportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Cursor = Cursors.Default;

            commentsTextBox.Enabled = true;
            _acceptButton.Enabled = true;

            if (e.Error != null)
            {
                MessageBox.Show(
                    "Unable to report error. " + e.Error.Message,
                    "Report Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Allow user to retry.
                return;
            }

            Close();
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (restartCheckBox.Checked)
            {
                Process.Start(executablePath);
            }
        }
    }
}