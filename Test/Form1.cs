using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private Utils.Ftp.Ftp _ftp = new Utils.Ftp.Ftp();
        public Form1()
        {
            InitializeComponent();
            _ftp.FtpMessages += new Utils.Ftp.FtpMessagesEventHandler(_ftp_FtpMessages);
        }

        void _ftp_FtpMessages(object sender, Utils.Ftp.FtpMessagesEventArgs e)
        {
            this.textBox5.Text += e.Message ;
            this.textBox5.SelectionStart = this.textBox5.Text.Length;
            this.textBox5.ScrollToCaret();
        }

        private void click(object sender, EventArgs e)
        {
            if (sender.Equals(this.buttonConnetti))
            {
                _ftp.FtpServer = this.textBox1.Text;
                _ftp.UserName = this.textBox2.Text;
                _ftp.Password = this.textBox3.Text;
                _ftp.Port = int.Parse(this.textBox4.Text);
                _ftp.Connect();
            }
            else if (sender.Equals(this.buttonEsci))
            {
                if (_ftp.IsConnected())
                    _ftp.Close();
                this.Close();
            }
            else if (sender.Equals(this.buttonGet))
            {
                MessageBox.Show("Preleva un file su remoto");
                _ftp.Download("filedainviare.txt", Utils.ClassUtils.ApplicationDir() + "\\filedainviare1.txt", false);
            }
            else if (sender.Equals(this.buttonList))
            {
                MessageBox.Show("Effettua la list su remoto");
                _ftp.List("*.*");
            }
            else if (sender.Equals(this.buttonSend))
            {
                MessageBox.Show("Invia un file su remoto");
                _ftp.Upload(Utils.ClassUtils.ApplicationDir() + "\\filedainviare.txt", "filedainviare.txt", false);
            }
        }
    }
}