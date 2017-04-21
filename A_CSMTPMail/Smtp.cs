using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

namespace Utils.Email
{
	public class Smtp : Component
	{

        public delegate void SmtpMessagesEventHandler(object sender, SmtpMessagesEventArgs e);
        public class SmtpMessagesEventArgs : EventArgs
        {
            private string message;
            public SmtpMessagesEventArgs(string message)
            {
                this.message = message;
            }
            public string Message
            {
                get
                {
                    return message;
                }
            }
        }

        private Utils.Logger _logger = null;

        public event SmtpMessagesEventHandler SmtpMessages = null;

		private const int m_smtpport = 25;

        private bool m_requireAuthorization = false;

		private string m_user = "";

		private string m_password = "";

		private string m_server = "";

		private string m_to = "";

		private string m_from = "";

		private string m_cc = "";

		private string m_bcc = "";

		private string m_subject = "";

		private string m_messageText = "";

		private object m_attachment;

		private string[] m_attachments;

		private ClientSocket m_clientSocket = new ClientSocket(null);

		//private Timer m_DemoTimer = new Timer();

		public readonly static string HELO_REPLY;

		public readonly static string QUIT;

		public readonly static string AUTH_SUCCESSFUL;

		public readonly static string OK;

		public readonly static string SERVER_CHALLENGE;

		public readonly static string START_INPUT;

        public Utils.Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

		public object Attachments
		{
			get
			{
				return this.m_attachment;
			}
			set
			{
				this.m_attachment = value;
				if (this.m_attachment != null)
				{
					if (this.m_attachment is string)
					{
						char[] chrArray = new char[] { ',' };
						this.m_attachments = this.m_attachment.ToString().Split(chrArray);
						return;
					}
					if (this.m_attachment.GetType().IsArray)
					{
						int length = ((Array)this.m_attachment).GetLength(0);
						this.m_attachments = new string[length];
						for (int i = 0; i < length; i++)
						{
							this.m_attachments[i] = ((Array)this.m_attachment).GetValue(i).ToString();
						}
						return;
					}
					this.m_attachments = null;
				}
			}
		}

		public string BCC
		{
			get
			{
				return this.m_bcc;
			}
			set
			{
				this.m_bcc = value;
			}
		}

		public string CC
		{
			get
			{
				return this.m_cc;
			}
			set
			{
				this.m_cc = value;
			}
		}

		public string From
		{
			get
			{
				return this.m_from;
			}
			set
			{
				this.m_from = value;
			}
		}

		public string MessageText
		{
			get
			{
				return this.m_messageText;
			}
			set
			{
				this.m_messageText = value;
			}
		}

		public string Password
		{
			get
			{
				return this.m_password;
			}
			set
			{
				this.m_password = value;
			}
		}

		public string Server
		{
			get
			{
				return this.m_server;
			}
			set
			{
				this.m_server = value;
			}
		}

		public string Subject
		{
			get
			{
				return this.m_subject;
			}
			set
			{
				this.m_subject = value;
			}
		}

		public int Timeout
		{
			get
			{
				return this.m_clientSocket.Timeout;
			}
			set
			{
				this.m_clientSocket.Timeout = value;
			}
		}

		public string To
		{
			get
			{
				return this.m_to;
			}
			set
			{
				this.m_to = value;
			}
		}

        public bool RequireAuthorization
        {
            get { return m_requireAuthorization; }
            set { m_requireAuthorization = value; }

        }

		public string UserName
		{
			get
			{
				return this.m_user;
			}
			set
			{
				this.m_user = value;
			}
		}

		static Smtp()
		{
			Smtp.HELO_REPLY = "220";
			Smtp.QUIT = "221";
			Smtp.AUTH_SUCCESSFUL = "235";
			Smtp.OK = "250";
			Smtp.SERVER_CHALLENGE = "334";
			Smtp.START_INPUT = "354";
		}

		public Smtp()
		{
            _logger = new Logger("smtplog{0}.txt", "stmplog*.txt");
		}

		private bool AuthLogin(string username, string password)
		{
			if (username == null || username.Length <= 0 || password == null || password.Length <= 0)
			{
				return true;
			}
            if(SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs("Send->AUTH LOGIN"));

			this.m_clientSocket.Send("AUTH LOGIN");
            string response = "";
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.SERVER_CHALLENGE, out response))
			{
                if(SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));

                this.m_clientSocket.Close();
				return false;
			}
            if(SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));

            byte[] bytes = Encoding.ASCII.GetBytes(username.ToCharArray());
            if(SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs("Send->" + Convert.ToBase64String(bytes, 0, (int)bytes.Length)));

            this.m_clientSocket.Send(Convert.ToBase64String(bytes, 0, (int)bytes.Length));
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.SERVER_CHALLENGE, out response))
			{
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));
                
                this.m_clientSocket.Close();
				return false;
			}
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));
            
            bytes = Encoding.ASCII.GetBytes(password.ToCharArray());
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs("Send->" + Convert.ToBase64String(bytes, 0, (int)bytes.Length)));
            this.m_clientSocket.Send(Convert.ToBase64String(bytes, 0, (int)bytes.Length));
			if (this.m_clientSocket.CheckSmtpResponse(Smtp.AUTH_SUCCESSFUL, out response))
			{
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));
                return true;
			}
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));
            this.m_clientSocket.Close();
			return false;
		}

        //private void m_DemoTimer_Tick(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Thank you for trying Mooseworks Software, LLC Email controls");
        //}

		public bool Send()
		{
            _logger.Start();

			return this.SendMessage();
		}

		private bool SendMessage()
		{
            _logger.WriteLogFile("Info: Sono in SendMessage");
			int length = 0;
			string str;
			string str1;
            string response = "";
            string s = "";
			this.m_clientSocket.GetClientSocket(this.m_server, 25);
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.HELO_REPLY, out response))
			{
                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                this.m_clientSocket.Close();
				return false;
			}

            s = "Recv<-" + response;
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            s = "Send->" + string.Format("EHLO {0}", Dns.GetHostName());
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            this.m_clientSocket.Send(string.Format("EHLO {0}", Dns.GetHostName()));
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response))
			{
                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                s = "Send->" + string.Format("HELO {0}", Dns.GetHostName());
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                this.m_clientSocket.Send(string.Format("HELO {0}", Dns.GetHostName()));
				if (!this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response))
				{
                    s = "Recv<-" + response;
                    if (SmtpMessages != null)
                        SmtpMessages(this, new SmtpMessagesEventArgs(s));
                    _logger.WriteLogFile(s);

                    this.m_clientSocket.Close();
					return false;
				}

                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

            }

            s = "Recv<-" + response;
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            if (RequireAuthorization)
            {
                if (!this.AuthLogin(this.m_user, this.m_password))
                {
                    return false;
                }
            }

            s = "Send->" + string.Format("MAIL From: <{0}>", this.m_from);
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            this.m_clientSocket.Send(string.Format("MAIL From: <{0}>", this.m_from));
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response))
			{
                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                this.m_clientSocket.Close();
				return false;
			}

            s = "Recv<-" + response;
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat("From: ", this.m_from, "\r\n"));
			stringBuilder.Append("To: ");
			char[] chrArray = new char[] { ';' };
			string[] strArrays = this.m_to.Split(chrArray);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{

                s = "Send->" + string.Format("RCPT TO: <{0}>", strArrays[i]);
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                this.m_clientSocket.Send(string.Format("RCPT TO: <{0}>", strArrays[i]));
				this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response);

                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                StringBuilder stringBuilder1 = stringBuilder;
				str = (i > 0 ? "," : "");
				stringBuilder1.Append(str);
				stringBuilder.Append(strArrays[i]);
			}
			stringBuilder.Append("\r\n");
			if (this.m_cc != "")
			{
				stringBuilder.Append("Cc: ");
				char[] chrArray1 = new char[] { ';' };
				strArrays = this.m_cc.Split(chrArray1);
				for (int j = 0; j < (int)strArrays.Length; j++)
				{
                    s = "Send->" + string.Format("RCPT TO: <{0}>", strArrays[j]);
                    if (SmtpMessages != null)
                        SmtpMessages(this, new SmtpMessagesEventArgs(s));
                    _logger.WriteLogFile(s);

                    this.m_clientSocket.Send(string.Format("RCPT TO: <{0}>", strArrays[j]));
					this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response);

                    s = "Recv<-" + response;
                    if (SmtpMessages != null)
                        SmtpMessages(this, new SmtpMessagesEventArgs(s));
                    _logger.WriteLogFile(s);

                    StringBuilder stringBuilder2 = stringBuilder;
					str1 = (j > 0 ? "," : "");
					stringBuilder2.Append(str1);
					stringBuilder.Append(strArrays[j]);
				}
				stringBuilder.Append("\r\n");
			}
			if (this.m_bcc != "")
			{
				char[] chrArray2 = new char[] { ';' };
				strArrays = this.m_bcc.Split(chrArray2);
				for (int k = 0; k < (int)strArrays.Length; k++)
				{
                    s = "Send->" + string.Format("RCPT TO: <{0}>", strArrays[k]);
                    if (SmtpMessages != null)
                        SmtpMessages(this, new SmtpMessagesEventArgs(s));
                    _logger.WriteLogFile(s);

                    this.m_clientSocket.Send(string.Format("RCPT TO: <{0}>", strArrays[k]));
					this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response);

                    s = "Recv<-" + response;
                    if (SmtpMessages != null)
                        SmtpMessages(this, new SmtpMessagesEventArgs(s));
                    _logger.WriteLogFile(s);
                }
			}
			stringBuilder.Append("Date: ");
			DateTime now = DateTime.Now;
			stringBuilder.Append(now.ToString("ddd, d MMM yyyy H:m:s z"));
			stringBuilder.Append("\r\n");
			stringBuilder.Append(string.Concat("Subject: ", this.m_subject, "\r\n"));
			stringBuilder.Append("X-Mailer: SMTPDirect v1\r\n");
			string mMessageText = this.m_messageText;
			if (!mMessageText.EndsWith("\r\n"))
			{
				mMessageText = string.Concat(mMessageText, "\r\n");
			}
			if (this.m_attachments != null)
			{
				stringBuilder.Append("MIME-Version: 1.0\r\n");
				stringBuilder.Append("Content-Type: multipart/mixed; boundary=unique-boundary-1\r\n");
				stringBuilder.Append("\r\n");
				stringBuilder.Append("This is a multi-part message in MIME format.\r\n");
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append("--unique-boundary-1\r\n");
				stringBuilder3.Append("Content-Type: text/plain\r\n");
				stringBuilder3.Append("Content-Transfer-Encoding: 7Bit\r\n");
				stringBuilder3.Append("\r\n");
				stringBuilder3.Append(string.Concat(mMessageText, "\r\n"));
				stringBuilder3.Append("\r\n");
				string[] mAttachments = this.m_attachments;
				for (int l = 0; l < (int)mAttachments.Length; l++)
				{
					string str2 = mAttachments[l];
					if (str2 != "")
					{
						FileInfo fileInfo = new FileInfo(str2);
						stringBuilder3.Append("--unique-boundary-1\r\n");
						stringBuilder3.Append(string.Concat("Content-Type: application/octet-stream; file=", fileInfo.Name, "\r\n"));
						stringBuilder3.Append("Content-Transfer-Encoding: base64\r\n");
						stringBuilder3.Append(string.Concat("Content-Disposition: attachment; filename=", fileInfo.Name, "\r\n"));
						stringBuilder3.Append("\r\n");
						FileStream fileStream = fileInfo.OpenRead();
						//byte[] numArray = new byte[checked((IntPtr)fileStream.Length)];
                        byte[] numArray = new byte[checked((int)fileStream.Length)];
						fileStream.Read(numArray, 0, (int)fileStream.Length);
						fileStream.Close();
						string base64String = Convert.ToBase64String(numArray, 0, (int)numArray.Length);
						for (int m = 0; m < base64String.Length; m = m + length)
						{
							length = 100;
							if (base64String.Length - (m + length) < 0)
							{
								length = base64String.Length - m;
							}
							stringBuilder3.Append(base64String.Substring(m, length));
							stringBuilder3.Append("\r\n");
						}
						stringBuilder3.Append("\r\n");
					}
				}
				mMessageText = stringBuilder3.ToString();
			}

            s = "Send->DATA";
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            this.m_clientSocket.Send("DATA");
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.START_INPUT, out response))
			{
                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                this.m_clientSocket.Close();
				return false;
			}

            s = "Recv<-" + response;
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            stringBuilder.Append("\r\n");
			stringBuilder.Append(mMessageText);
			stringBuilder.Append(".\r\n");
			stringBuilder.Append("\r\n");

            s = stringBuilder.ToString();
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            this.m_clientSocket.Send(stringBuilder.ToString());
			if (!this.m_clientSocket.CheckSmtpResponse(Smtp.OK, out response))
			{
                s = "Recv<-" + response;
                if (SmtpMessages != null)
                    SmtpMessages(this, new SmtpMessagesEventArgs(s));
                _logger.WriteLogFile(s);

                this.m_clientSocket.Close();
				return false;
			}
            s = "Recv<-" + response;
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            s = "QUIT";
            if (SmtpMessages != null)
                SmtpMessages(this, new SmtpMessagesEventArgs(s));
            _logger.WriteLogFile(s);

            this.m_clientSocket.Send("QUIT");
			this.m_clientSocket.CheckSmtpResponse(Smtp.QUIT, out response);
            //if (SmtpMessages != null)
            //    SmtpMessages(this, new SmtpMessagesEventArgs("Recv<-" + response));
            this.m_clientSocket.Close();
			return true;
		}
	}
}