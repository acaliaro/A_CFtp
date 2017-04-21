using System;
using System.Text;

using System.Collections.Generic;

namespace Utils.SMTP
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class SmtpServer
	{
		private string sServer;
		private int iPort;
		private bool bAuth=false;
		private string sUser;
		private string sPass;
		private StringBuilder sbLog=null;
		private bool bLog=true;
		private SocketHelper helper=null;
		private TalkState state=TalkState.WaitInit;

        public delegate void SMTPMessagesEventHandler(object sender, SMTPMessagesEventArgs e);
        public class SMTPMessagesEventArgs : EventArgs
        {
            private string message;
            public SMTPMessagesEventArgs(string message)
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

        public event SMTPMessagesEventHandler SMTPMessages = null;

		public enum TalkState
		{
			WaitInit,Initialized,SignedIn,LoginFailed,Ended
		}

		protected SocketHelper Helper
		{
			get
			{
				if(helper==null)
					helper=new SocketHelper(this.Server,this.Port);

				return helper;
			}
		}

		public bool LogEvent
		{
			get
			{
				return bLog;
			}
			set
			{
				bLog=value;
			}
		}

		public string LogMessage
		{
			get
			{
				if(this.LogEvent)
					return sbLog.ToString();
				else
					return "";
			}
		}

		public string Server
		{
			get
			{
				return sServer;
			}
			set
			{
				sServer=value;
			}
		}

		public int Port
		{
			get
			{
				return iPort;
			}
			set
			{
				iPort=value;
			}
		}

		public bool RequireAuthorization
		{
			get
			{
				return bAuth;
			}
			set
			{
				bAuth=value;
			}
		}

		public string AuthUser
		{
			get
			{
				return sUser;
			}
			set
			{
				sUser=value;
			}
		}

		public string AuthPass
		{
			get
			{
				return sPass;
			}
			set
			{
				sPass=value;
			}
		}

		public SmtpServer()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Init()
		{
			Log(helper.GetFullResponse());
            string command = "EHLO " + this.Server;
			Helper.SendCommand(command);
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendData ->" + command));

			Log(Helper.GetFullResponse());
			state=TalkState.Initialized;
		}

		public bool Login()
		{
			if(this.RequireAuthorization)
			{
                string command = "AUTH LOGIN";
                Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
				Log(Helper.GetFullResponse());

                command = Convert.ToBase64String(
                    Encoding.Default.GetBytes(this.AuthUser));
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Helper.SendCommand(command);				
                Log(Helper.GetFullResponse());

                command = Convert.ToBase64String(
                    Encoding.Default.GetBytes(this.AuthPass));
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Helper.SendCommand(command);
				Log(Helper.GetFullResponse());

				if(Helper.GetResponseState()==235)
				{
					state=TalkState.SignedIn;
					return true;
				}
				else
				{
					state=TalkState.LoginFailed;
					return false;
				}
//				return (Helper.GetResponseState()==235);
			}
			else
			{
				state=TalkState.SignedIn;
				return true;
			}
		}

		public void SendKeep(MailMessage msg)
		{
			if((state==TalkState.Initialized && !this.RequireAuthorization) || state==TalkState.SignedIn)
			{
                string command = "MAIL From:" + msg.From;
                Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
				Log(Helper.GetFullResponse());

                command = "RCPT To:" + msg.To;
				Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Log(Helper.GetFullResponse());

                command = "DATA";
                Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Log(Helper.GetFullResponse());

                command = msg.ToString();
                Helper.SendData(Encoding.Default.GetBytes(msg.ToString()));
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendData ->" + command));
                
                Helper.SendCommand(".");
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->."));
                Log(Helper.GetFullResponse());
			}
		}

		public void Quit()
		{
			Helper.SendCommand("QUIT");
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->QUIT"));
            Helper.Close();
			state=TalkState.Ended;
		}

		public bool Send(MailMessage msg)
		{
			if(state==TalkState.Ended)
				return false;

			Log(Helper.GetFullResponse());
            string command = "EHLO " + this.Server;
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
            Helper.SendCommand(command);
			Log(Helper.GetFullResponse());

			if(this.RequireAuthorization)
			{
                command = "AUTH LOGIN";
				Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Log(Helper.GetFullResponse());

                command = Convert.ToBase64String(
                    Encoding.Default.GetBytes(this.AuthUser));
                Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Log(Helper.GetFullResponse());

                command = Convert.ToBase64String(
                    Encoding.Default.GetBytes(this.AuthPass));
                Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Log(Helper.GetFullResponse());
				
                if(Helper.GetResponseState()!=235)
				{
                    if (SMTPMessages != null)
                        SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->QUIT" ));
                    Helper.SendCommand("QUIT");
					Helper.Close();
					state=TalkState.Ended;
					return false;
				}
			}

            command = "MAIL FROM: <" + msg.From + ">";
			Helper.SendCommand(command);
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
            Log(Helper.GetFullResponse());

            foreach (string addres in getAddress(msg.To))
            {
                command = "RCPT TO: <" + addres + ">";
                Helper.SendCommand(command);
                if (SMTPMessages != null)
                    SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->" + command));
                Log(Helper.GetFullResponse());
            }

            Helper.SendCommand("DATA");
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->DATA" ));
            Log(Helper.GetFullResponse());

            command = msg.ToString();
			Helper.SendData(Encoding.Default.GetBytes(command));
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendData ->" + command));
            
            Helper.SendCommand(".");
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->."));
            Log(Helper.GetFullResponse());
			
            Helper.SendCommand("QUIT");
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("SendCommand ->QUIT"));
            Helper.Close();
			state=TalkState.Ended;
			return true;
		}

		private void Log(string msg)
		{
            if (SMTPMessages != null)
                SMTPMessages(this, new SMTPMessagesEventArgs("Response <-" +  msg));

			if(this.LogEvent)
			{
				if(sbLog==null)
					sbLog=new StringBuilder();

				sbLog.Append(string.Format("{0}\r\n",msg));
			}
		}

        private List<string> getAddress(string address)
        {
            List<string> ritorno = new List<string>();
            char[] separator = { ';' };
            string[] add = address.Split(separator);

            foreach (string s in add)
            {
                if (s != "")
                    ritorno.Add(s);
            }
            return ritorno;
        }
	}
}
