using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;
using System.Net;

namespace Utils.SMTP
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class SocketHelper
	{

		private TcpClient client=null;
		private NetworkStream stream=null;
		private StreamReader reader=null;
		private StreamWriter writer=null;

		private string resp="";
		private int state=-1;
		private string msg="";
		private string srv="";
		private int pt=25;

		public SocketHelper(string name,int port)
		{
			//
			// TODO: Add constructor logic here
			//
			srv=name;
			pt=port;

			client=new TcpClient(name,port);
            
            //client = new TcpClient(ipEndPoit);
			stream=client.GetStream();
			reader=new StreamReader(stream);
			writer=new StreamWriter(stream);


		}

//		public SocketHelper(TcpClient tc)
//		{
//			client=tc;
//			stream=client.GetStream();
//			reader=new StreamReader(stream);
//			writer=new StreamWriter(stream);
//		}

		public void SendData(byte[] bts)
		{
			if(GetResponseState()!=221)
			{
				stream.Write(bts,0,bts.Length);
				stream.Flush();

			}
		}

		public void SendCommand(string cmd)
		{
			if(GetResponseState()!=221)
			{
                writer.WriteLine(cmd);
                writer.Flush();

			}
		}

		public string RecvResponse()
		{
			if(GetResponseState()!=221)
                resp = reader.ReadLine();

			else
				resp="221 closed!";

			return resp;
		}

		public int GetResponseState()
		{
			if(resp!=null && resp.Length>=3 && IsNumber(resp[0]) && IsNumber(resp[1]) && IsNumber(resp[2]))
				state=Convert.ToInt32(resp.Substring(0,3));

			return state;
		}

		public string GetResponseMessage()
		{
			if(resp!=null && resp.Length>4)
				msg=resp.Substring(4);
			else
				msg="";

			return msg;
		}

		private bool IsNumber(char c)
		{
			return c>='0' && c<='9';
		}

		public string GetFullResponse()
		{
			StringBuilder sb=new StringBuilder();
			sb.Append(RecvResponse());
            if (GetResponseState() == 334)
            {
                string responseMessage = GetResponseMessage();
                byte[] response = Convert.FromBase64String(responseMessage);

                sb.Append(string.Format("[{0}]",
                    System.Text.Encoding.ASCII.GetString(
                    response,0,response.GetLength(0))));
            }

			sb.Append("\r\n");
			while(HaveNextResponse())
			{
				sb.Append(RecvResponse());

				sb.Append("\r\n");
			}
			return sb.ToString();
		}

		public bool HaveNextResponse()
		{
			if(GetResponseState()>-1)
			{
				if(resp!=null && resp.Length>=4 && resp[3]!=' ')
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public void Close()
		{
            if (client != null)
            {
                try
                {
                    client.Close();
                    client = null;
                }
                catch { }
            }

            if (stream != null)
            {
                try
                {
                    stream.Close();
                    stream = null;
                }
                catch { }
            }

            if (reader != null)
            {
                try
                {
                    reader.Close();
                    reader = null;
                }
                catch { }
            }

            if (writer != null)
            {
                try
                {
                    writer.Close();
                    writer = null;
                }
                catch { }
            }
		}

		public void ReConnect()
		{
			Connect(srv,pt);
		}

		public void Connect(string host,int port)
		{

			client.Connect(host,port);
            //client.Connect(ipEndPoit);
			stream=client.GetStream();
			reader=new StreamReader(stream);
			writer=new StreamWriter(stream);
			srv=host;
			pt=port;
		}
	}
}
