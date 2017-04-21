using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Windows.Forms;

namespace Utils.Email
{
	internal class ClientSocket
	{
		private const int MAX_BUFFER_READ_SIZE = 256;

		private System.Net.Sockets.Socket m_socket;

		private Pop3StateObject m_pop3State;

		private ManualResetEvent m_manualEvent = new ManualResetEvent(false);

		//private System.Windows.Forms.Timer m_timer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Timeout in secondi per la ricezione delle risposte
        /// </summary>
        public int Timeout { get; set; }

        public System.Net.Sockets.Socket Socket
		{
			get
			{
				return this.m_socket;
			}
		}

		public System.Text.StringBuilder StringBuilder
		{
			get
			{
				return this.m_pop3State.sb;
			}
		}

        //public int Timeout
        //{
        //    get
        //    {
        //        return this.m_timer.Interval / 1000;
        //    }
        //    set
        //    {
        //        this.m_timer.Interval = value * 1000;
        //    }
        //}

		public ClientSocket(System.Net.Sockets.Socket socket)
		{
			this.m_socket = socket;
			this.m_pop3State = new Pop3StateObject();
			this.m_pop3State.workSocket = this.m_socket;
			this.m_pop3State.sb = new System.Text.StringBuilder();
            Timeout = 20;
            //this.m_timer.Interval = 20000;
            //this.m_timer.Tick += new EventHandler(this.m_timer_Tick);
		}

		public bool CheckSmtpResponse(string response_expected, out string response)
		{
            response = "";
			byte[] numArray = new byte[1024];

            DateTime dateTimeStart = DateTime.Now;
            while (this.m_socket.Available <= 0)
            {

                TimeSpan t = DateTime.Now - dateTimeStart;

                if (t.Seconds >= Timeout)
                {
                    response = "TIMEOUT!";
                    return false;
                }
            }

            //this.m_timer.Enabled = true;
            //do
            //{
            //    if (this.m_socket.Available != 0)
            //    {
            //        break;
            //    }
            //    Application.DoEvents();
            //    Thread.Sleep(100);
            //}
            //while (this.m_timer.Enabled);
            //this.m_timer.Enabled = false;
			if (this.m_socket.Available > 0)
			{
				this.m_socket.Receive(numArray, 0, this.m_socket.Available, SocketFlags.None);
				string str = Encoding.ASCII.GetString(numArray, 0, (int)numArray.Length);
                response = str;
                
                // Rimuovo eventuali null...
                int posNull = response.IndexOf('\0');
                if (posNull >= 0)
                    response = response.Remove(posNull, response.Length - posNull); 
                
                if (str.IndexOf(response_expected) != -1)
                {
                    return true;
                }
			}


			return false;
		}

		public void Clear()
		{
			this.m_pop3State = null;
		}

		public void Close()
		{
			if (this.m_socket != null)
			{
				this.m_socket.Close();
				this.m_socket = null;
			}
		}

		public void GetClientSocket(string server, int port)
		{
			try
			{
				try
				{
					char[] chrArray = new char[] { '.' };
					server.Split(chrArray);
					Regex regex = new Regex("^(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])$");
					Match match = regex.Match(server);
					if (match.Success && match.Groups.Count >= 5)
					{
						long num = (long)0;
						for (int i = 0; i < 4; i++)
						{
							num = num + Convert.ToInt64(match.Groups[i + 1].Value) * (long)Math.Pow(256, (double)i);
						}
						IPAddress pAddress = new IPAddress(num);
						IPEndPoint pEndPoint = new IPEndPoint(pAddress, port);
						System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(pEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
						socket.Connect(pEndPoint);
						if (socket.Connected)
						{
							this.m_socket = socket;
						}
					}
				}
				catch
				{
				}
				if (this.m_socket == null)
				{
					IPHostEntry hostEntry = null;
					hostEntry = Dns.GetHostEntry(server);
					IPAddress[] addressList = hostEntry.AddressList;
					int num1 = 0;
					while (num1 < (int)addressList.Length)
					{
						IPAddress pAddress1 = addressList[num1];
						IPEndPoint pEndPoint1 = new IPEndPoint(pAddress1, port);
						System.Net.Sockets.Socket socket1 = new System.Net.Sockets.Socket(pEndPoint1.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
						socket1.Connect(pEndPoint1);
						if (!socket1.Connected)
						{
							num1++;
						}
						else
						{
							this.m_socket = socket1;
							break;
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new Pop3Exception(string.Concat("Error : connecting to ", server, " - ", exception.ToString()));
			}
			if (this.m_socket == null)
			{
				throw new Pop3Exception(string.Concat("Error : connecting to ", server));
			}
		}

		public string GetPop3String()
		{
			if (this.m_socket == null)
			{
				throw new Pop3Exception("Connection to POP3 server is closed");
			}
			byte[] numArray = new byte[256];
			string str = null;
			try
			{
				int num = this.m_socket.Receive(numArray, (int)numArray.Length, SocketFlags.None);
				str = Encoding.ASCII.GetString(numArray, 0, num);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new Pop3Exception(exception.ToString());
			}
			return str;
		}

        //private void m_timer_Tick(object sender, EventArgs e)
        //{
        //    this.m_manualEvent.Set();
        //    this.m_timer.Enabled = false;
        //    throw new Pop3Exception("Recieve Timeout error");
        //}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				Pop3StateObject asyncState = (Pop3StateObject)ar.AsyncState;
				System.Net.Sockets.Socket socket = asyncState.workSocket;
				int num = socket.EndReceive(ar);
				if (num > 0)
				{
					asyncState.sb.Append(Encoding.ASCII.GetString(asyncState.buffer, 0, num));
					this.StartReceiveAgain(asyncState.sb.ToString());
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.m_manualEvent.Set();
				//this.m_timer.Enabled = false;
				throw new Pop3Exception(string.Concat("RecieveCallback error", exception.ToString()));
			}
		}

		public void Send(string data)
		{
			if (this.m_socket == null)
			{
				throw new Pop3Exception("Pop3 connection is closed");
			}
			try
			{
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(data, "\r\n"));
				this.m_socket.Send(bytes);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new Pop3Exception(exception.ToString());
			}
		}

		public void StartReceive()
		{
			this.m_socket.BeginReceive(this.m_pop3State.buffer, 0, 4096, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.m_pop3State);
			//this.m_timer.Enabled = true;
			this.m_manualEvent.WaitOne();
			//this.m_timer.Enabled = false;
		}

		private void StartReceiveAgain(string data)
		{
			if (data.EndsWith("\r\n.\r\n"))
			{
				this.m_manualEvent.Set();
				return;
			}
			this.m_socket.BeginReceive(this.m_pop3State.buffer, 0, 4096, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.m_pop3State);
		}
	}
}