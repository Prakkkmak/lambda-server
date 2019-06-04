using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Lambda.Utils
{
    class Bug
    {
        public MailMessage Mail { get; set; }
        public string Text { get; set; }
        public string SenderName { get; set; }

        public Bug(string text, string sender)
        {
            Text = text;
            SenderName = sender;
            Mail = new MailMessage(BugMail, TrelloMail)
            {
                Subject = Text,
                Body = SenderName
            };
        }


        public void Send()
        {
            SmtpClient.Send(Mail);
        }
        public void SendAsync()
        {
            SmtpClient.SendAsync(Mail, null);
        }

        public static SmtpClient InitSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            string password = "]9>xPXf&M<hw$w;c";
            string frommail = "lambdarenewbug@gmail.com";
            client.Credentials = new NetworkCredential(frommail, password);
            return client;
        }

        public static SmtpClient SmtpClient = InitSmtpClient();
        public static string TrelloMail = "prakkmak+sna7gsu1oqlcujrmfuxe@boards.trello.com";
        public static string BugMail = "lambdarenewbug@gmail.com";
    }
}
