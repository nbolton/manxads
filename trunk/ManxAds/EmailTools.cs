using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace ManxAds
{
    public class EmailTools
    {
        public static void SendMessage(MailMessage message)
        {
            SmtpClient client = new SmtpClient(LocalSettings.MasterSmtpServer);

            client.Credentials = new NetworkCredential(
                LocalSettings.MasterSmtpUsername,
                LocalSettings.MasterSmtpPassword);

            writeToLog(message);
            client.Send(message);
        }

        public static void SendMessage(MailMessage message, Page page)
        {
            SendMessage(message);
        }

        private static void writeToLog(MailMessage message)
        {
            using (SqlConnection connection = new SqlConnection(LocalSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand("EmailLogWrite", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("toEmail", (message.To.Count != 0) ? message.To[0].Address : (object)DBNull.Value);
                command.Parameters.AddWithValue("fromEmail", (message.From != null) ? message.From.Address : (object)DBNull.Value);
                command.Parameters.AddWithValue("senderEmail", (message.Sender != null) ? message.Sender.Address : (object)DBNull.Value);
                command.Parameters.AddWithValue("subject", message.Subject);
                command.Parameters.AddWithValue("body", message.Body);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //private static void writeToLog(MailMessage message, Page page)
        //{
        //    FileInfo logFileInfo = new FileInfo(page.Server.MapPath(LocalSettings.EmailLogPath));

        //    StreamWriter writer = new StreamWriter(logFileInfo.FullName, true);
        //    string format = "{0},{1},{2},{3},{4}";

        //    if (writer.BaseStream.Length == 0)
        //    {
        //        string header = string.Format(
        //            format, "Time", "To", "From", "Sender", "Subject");

        //        writer.WriteLine(header);
        //    }

        //    string logLine = string.Format(
        //        format,
        //        DateTime.Now.ToString("dd-MM-yy hh:mm:ss"),
        //        (message.To.Count != 0) ? message.To[0].Address : String.Empty,
        //        (message.From != null) ? message.From.Address : String.Empty,
        //        (message.Sender != null) ? message.Sender.Address : String.Empty,
        //        message.Subject);

        //    writer.WriteLine(logLine);
        //    writer.Close();
        //}
    }
}
