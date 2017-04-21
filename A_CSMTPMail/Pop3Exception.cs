using System;

namespace Utils.Email
{
	public class Pop3Exception : Exception
	{
		private string m_exceptionString;

		public override string Message
		{
			get
			{
				return this.m_exceptionString;
			}
		}

		public Pop3Exception()
		{
			this.m_exceptionString = null;
		}

		public Pop3Exception(string exceptionString)
		{
			this.m_exceptionString = exceptionString;
		}

		public Pop3Exception(string exceptionString, Exception ex) : base(exceptionString, ex)
		{
		}

		public override string ToString()
		{
			return this.m_exceptionString;
		}
	}
}