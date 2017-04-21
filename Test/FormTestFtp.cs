using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class FormTestFtp : Form
    {

        private Utils.Ftp.Ftp ftp = new Utils.Ftp.Ftp();
        public FormTestFtp()
        {
            InitializeComponent();
        }

         private void FormTestFtp_Load(object sender, EventArgs e)
        {

            this.ftp.FtpServer = "ftp.rebex.net";
            this.ftp.UserName = "anonymous";
            this.ftp.Password = "any";
            this.ftp.Port = 21;
            //this.ftp.TextBox = this.textBox1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.button1))
            {
                this.ftp.FtpServer = "ftp.rebex.net";
                this.ftp.UserName = "anonymous";
                this.ftp.Password = "any";
                this.ftp.Port = 21;
                //this.ftp.TextBox = this.textBox1;
            }
            else if (sender.Equals(this.button4))
            {
                this.ftp.FtpServer = "ftp.digiland.it";
                this.ftp.UserName = "acaliaro";
                this.ftp.Password = "ireland";
                this.ftp.Port = 21;
                //t/his.ftp.TextBox = this.textBox1;
            }
            else if (sender.Equals(this.button5))
            {
                this.ftp.FtpServer = "192.168.0.105";
                this.ftp.UserName = "alessandro";
                this.ftp.Password = "ireland";
                this.ftp.Port = 21;
                //this.ftp.TextBox = this.textBox1;
            }
            else if (sender.Equals(this.button7))
            {
                this.ftp.FtpServer = "project4.pulse.com.ua";
                this.ftp.UserName = "mobileuser";
                this.ftp.Password = "qazwsx";
                this.ftp.Port = 21;
               this.ftp.UseSsl = true;
                //this.ftp.TextBox = this.textBox1;
            }
            else if (sender.Equals(this.button8))
            {
                this.ftp.FtpServer = "192.168.0.6";
                this.ftp.UserName = "alessandro";
                this.ftp.Password = "ireland";
                this.ftp.Port = 21;
                this.ftp.UseSsl = false;
                //this.ftp.TextBox = this.textBox1;

            }
            this.ftp.Timeout = 60000;
            ftp.Connect();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Utils.Ftp.FtpList ftplist in ftp.List("*.*"))
                {
                    this.textBox1.Text += ftplist.FileName + "\r\n";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ftp.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ftp.Cd("home");

        }

        private void button10_Click(object sender, EventArgs e)
        {
            ftp.Download("anagrafiche.xml", "anagrafiche.xml", false);
        }
    }
}