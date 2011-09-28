using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Rensoft.ErrorReporting
{
    public class FormErrorHelper : ErrorHelper
    {
        public FormErrorHelper(string serviceUrl, string clientCode, Exception exception)
            : base(serviceUrl, clientCode, exception) { }

        public FormErrorHelper(string serviceUrl, string clientCode, Exception exception, IWebProxy proxy)
            : base(serviceUrl, clientCode, exception, proxy) { }

        public void LaunchReporter()
        {
            string tempDir = Environment.GetEnvironmentVariable("TEMP");
            string fileName = "RensoftErrorReport-" + Guid.NewGuid().ToString() + ".dat";
            string filePath = tempDir + "\\" + fileName;

            XmlDocument document = new XmlDocument();
            XmlElement rootElement = document.CreateElement("rensoftErrorReport");
            XmlElement serviceUrlElement = document.CreateElement("serviceUrl");
            XmlElement clientCodeElement = document.CreateElement("clientCode");
            XmlElement exceptionTypeElement = document.CreateElement("exceptionType");
            XmlElement exceptionDetailsElement = document.CreateElement("exceptionDetails");
            XmlElement executablePathElement = document.CreateElement("executablePath");
            XmlElement useSystemProxyElement = document.CreateElement("useSystemProxy");

            document.AppendChild(rootElement);
            rootElement.AppendChild(serviceUrlElement);
            rootElement.AppendChild(clientCodeElement);
            rootElement.AppendChild(exceptionTypeElement);
            rootElement.AppendChild(exceptionDetailsElement);
            rootElement.AppendChild(executablePathElement);
            rootElement.AppendChild(useSystemProxyElement);

            serviceUrlElement.InnerText = ServiceUrl;
            clientCodeElement.InnerText = ClientCode;
            exceptionTypeElement.InnerText = Exception.GetType().Name;
            exceptionDetailsElement.InnerText = ErrorMessage;
            executablePathElement.InnerText = Application.ExecutablePath;
            useSystemProxyElement.InnerText = UseSystemProxy.ToString();

            if ((Proxy is WebProxy) && (Proxy != null))
            {
                XmlElement proxyElement = document.CreateElement("proxy");
                rootElement.AppendChild(proxyElement);

                XmlAttribute proxyHost = document.CreateAttribute("host");
                XmlAttribute proxyPort = document.CreateAttribute("port");
                XmlAttribute proxyUdc = document.CreateAttribute("udc");

                proxyElement.Attributes.Append(proxyHost);
                proxyElement.Attributes.Append(proxyPort);
                proxyElement.Attributes.Append(proxyUdc);

                proxyHost.Value = ((WebProxy)Proxy).Address.Host;
                proxyPort.Value = ((WebProxy)Proxy).Address.Port.ToString();
                proxyUdc.Value = ((WebProxy)Proxy).UseDefaultCredentials.ToString();

                if (!((WebProxy)Proxy).UseDefaultCredentials && (Proxy.Credentials != null))
                {
                    XmlAttribute proxyUser = document.CreateAttribute("user");
                    XmlAttribute proxyPass = document.CreateAttribute("pass");
                    proxyElement.Attributes.Append(proxyUser);
                    proxyElement.Attributes.Append(proxyPass);
                    proxyUser.Value = ((NetworkCredential)Proxy.Credentials).UserName;
                    proxyPass.Value = ((NetworkCredential)Proxy.Credentials).Password;
                }
            }

            document.Save(filePath);

            FileInfo exeFileInfo = new FileInfo(Application.ExecutablePath);
            string errorReporter = exeFileInfo.DirectoryName + @"\Rensoft.ErrorReporting.Client.exe";

            if (!File.Exists(errorReporter))
            {
                MessageBox.Show(
                    "Could not start Rensoft Error Reporting. " +
                    "The file location of the application does not exist.\r\n\r\n" +
                    errorReporter, "Rensoft Error Reporting",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw new Exception("Could not start Rensoft Error Reporting.", Exception);
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show(
                    "Could not start Rensoft Error Reporting. " +
                    "The error detail file does not exist.\r\n\r\n" +
                    filePath, "Rensoft Error Reporting",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw new Exception("Could not start Rensoft Error Reporting.", Exception);
            }

            // Start the error reporting application to load dat file in file path argument.
            Process.Start(errorReporter, "\"" + filePath + "\"");
        }
    }
}
