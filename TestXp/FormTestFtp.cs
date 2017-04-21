using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestXp
{
    public partial class FormTestFtp : Form
    {
        public FormTestFtp()
        {
            InitializeComponent();
        }

		private Utils.Ftp.Ftp ftp = null;


		private void Form1_Load(object sender, EventArgs e)
		{

            this.comboBoxType.Items.Add(Utils.Ftp.Ftp.EnumType.Ascii);
            this.comboBoxType.Items.Add(Utils.Ftp.Ftp.EnumType.Ebcdic);
            this.comboBoxType.Items.Add(Utils.Ftp.Ftp.EnumType.Image);
            this.comboBoxType.SelectedIndex = 0;
            ftp = new Utils.Ftp.Ftp("", "", "", 21);
            if (System.IO.File.Exists(ftp.ConfigFile))
            {
                ftp.ReadConfigFile();
                this.textBoxServer.Text = ftp.FtpServer;
                this.textBoxUser.Text = ftp.UserName;
                this.textBoxPassword.Text = ftp.Password;
                this.textBoxRemoteFileName.Text = ftp.String2;
                this.textBoxLocalFileName.Text = ftp.String1;
            }
            //ftp.LogDirectoryName = @"C:\";

            ftp.FtpMessages += new Utils.Ftp.FtpMessagesEventHandler(ftp_FtpMessages);
            ftp.FtpTransferPercentage += new Utils.Ftp.FtpTransferPercentageEventHandler(ftp_FtpTrasnferPercentage);
        }
        
		private void closeFtp()
		{
			if (ftp != null)
			{
				if (ftp.IsConnected())
					ftp.Close();
			}
		}

		private void click(object sender, EventArgs e)
		{
			if (sender.Equals(this.buttonConnect))
			{
				try
				{
					if (this.buttonConnect.Text == "Connect")
					{
                        ftp.FtpServer = this.textBoxServer.Text;
                        ftp.UserName = this.textBoxUser.Text;
                        ftp.Password = this.textBoxPassword.Text;
                        ftp.Port = 21;
                        //ftp.TextBox = this.textBoxLog;
                        ftp.Timeout = 120000;
                        ftp.String1 = this.textBoxLocalFileName.Text;
                        ftp.String2 = this.textBoxRemoteFileName.Text;
                        ftp.WriteConfigFile();
						ftp.Connect();
						if (ftp.IsConnected())
							this.buttonConnect.Text = "Close connection";
					}
					else
					{
						closeFtp();
						this.buttonConnect.Text = "Connect";
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show("ERROR:" + ex.Message);
				}

			}
            else if (sender.Equals(this.buttonPwd))
            {
                try
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                            MessageBox.Show("Directory:" + ftp.Pwd());
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonWriteConfigFile))
            {
                try
                {
                    if (ftp != null)
                    {
                        ftp.FtpServer = this.textBoxServer.Text;
                        ftp.UserName = this.textBoxUser.Text;
                        ftp.Password = this.textBoxPassword.Text;
                        ftp.WriteConfigFile("test");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonReadConfigFile))
            {
                try
                {
                    if (ftp != null)
                    {
                        if (ftp.ExistConfigFile("test") == false)
                        {
                            MessageBox.Show("Config file is not present. Do a 'write'");
                            return;
                        }
                        ftp.ReadConfigFile("test");
                        this.textBoxUser.Text = ftp.UserName;
                        this.textBoxPassword.Text = ftp.Password;
                        this.textBoxServer.Text = ftp.FtpServer;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonCd))
            {
                try
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                        {
                            ftp.Cd(this.textBoxCd.Text);
                        }
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonMkdir))
            {
                try
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                        {
                            ftp.Mkdir(this.textBoxMkDir.Text);
                        }
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonHelp))
            {
                try
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                        {
                            ftp.Help();
                        }
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRORE :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonSendFile) || sender.Equals(this.buttonReceiveFile) ||
                sender.Equals(this.buttonAppendFile))
            {
                try
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                        {
                            if (sender.Equals(this.buttonSendFile))
                                ftp.Upload(this.textBoxLocalFileName.Text, this.textBoxRemoteFileName.Text, false);
                            else if (sender.Equals(this.buttonReceiveFile))
                                ftp.Download(this.textBoxRemoteFileName.Text, this.textBoxLocalFileName.Text, false);
                            else if (sender.Equals(this.buttonAppendFile))
                                ftp.Append(this.textBoxLocalFileName.Text, this.textBoxRemoteFileName.Text);
                        }
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }
            }
            else if (sender.Equals(this.buttonDir))
            {
                try
                {

                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                        {
                            if (ftp.List(this.textBoxRemoteDir.Text).Count == 0)
                            {
                                MessageBox.Show("File not found");
                            }
                        }
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR:" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }

            }
            else if (sender.Equals(this.buttonSendCmd))
            {
                try
                {

                    if (ftp != null)
                    {
                        if (ftp.IsConnected())
                        {
                            ftp.SendCommand(this.textBoxSendCommand.Text);
                        }
                        else
                            MessageBox.Show("Connect to server");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR :" + ex.Message);
                }
                finally
                {
                    if (ftp != null)
                    {
                        if (ftp.IsConnected() == false)
                            this.buttonConnect.Text = "Connect";
                    }
                }

            }
        }

        void ftp_FtpTrasnferPercentage(object sender, Utils.Ftp.FtpTransferPercentageEventArgs e)
        {
            this.progressBar2.Value = e.Percentage;
        }

        void ftp_FtpMessages(object sender, Utils.Ftp.FtpMessagesEventArgs e)
        {
            this.textBoxEvent.Text += e.Message;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ftp == null)
                return;

            if(ftp.IsConnected())
            {
                try
                {
                    switch (this.comboBoxType.Text[0])
                    {
                        case 'A':
                            ftp.Type(Utils.Ftp.Ftp.EnumType.Ascii);
                            break;
                        case 'E':
                            ftp.Type(Utils.Ftp.Ftp.EnumType.Ebcdic);
                            break;
                        case 'I':
                            ftp.Type(Utils.Ftp.Ftp.EnumType.Image);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}