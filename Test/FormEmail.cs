using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utils.Email;

namespace Test
{
    public partial class FormEmail : Form
    {
        public FormEmail()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //MailMessage msg = new MailMessage();
            //msg.From = tbFrom.Text;
            //msg.To = tbTo.Text;
            //msg.Subject = tbSubject.Text;
            //msg.Body = tbBody.Text;
            //msg.FilenameAttach = "\\my documents\\logs\\ftp\\prova.txt";
            //msg.MailFormat = MailMessage.BodyFormat.TEXT;
            //SmtpServer srv = new SmtpServer();
            //srv.SMTPMessages += new SmtpServer.SMTPMessagesEventHandler(srv_SMTPMessages);
            //srv.Server = tbServer.Text;
            //srv.Port = 25;
            //srv.RequireAuthorization = true;
            //srv.AuthUser = tbUser.Text;
            //srv.AuthPass = tbPass.Text;
            //srv.Send(msg);
            //tbLog.Text = srv.LogMessage;
        }

        //void srv_SMTPMessages(object sender, SmtpServer.SMTPMessagesEventArgs e)
        //{
        //    this.tbLog.Text += e.Message + "\r\n";
        //}
    }
}