using System;
using System.Net.Sockets;
using System.Text;

namespace Utils.Email
{
	internal class Pop3StateObject
	{
		public const int BufferSize = 4096;

		public Socket workSocket;

		public byte[] buffer = new byte[4096];

		public StringBuilder sb = new StringBuilder();

		public Pop3StateObject()
		{
		}
	}
}