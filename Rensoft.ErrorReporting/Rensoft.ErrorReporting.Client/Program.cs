using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Rensoft.ErrorReporting.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Allow previous application to close.
            Thread.Sleep(1000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ReportError(args[0]));
        }
    }
}