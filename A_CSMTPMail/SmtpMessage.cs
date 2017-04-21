using System;
using System.Collections;

namespace Utils.Email
{
	internal class SmtpMessage
	{
		private string to;

		private string @from;

		private string body;

		private string subject;

		private string cc;

		private string bcc;

		private IList attachments;

		public IList Attachments
		{
			get
			{
				return this.attachments;
			}
		}

		public string Bcc
		{
			get
			{
				return this.bcc;
			}
			set
			{
				this.bcc = value;
			}
		}

		public string Body
		{
			get
			{
				return this.body;
			}
			set
			{
				this.body = value;
			}
		}

		public string Cc
		{
			get
			{
				return this.cc;
			}
			set
			{
				this.cc = value;
			}
		}

		public string From
		{
			get
			{
				return this.@from;
			}
			set
			{
				this.@from = value;
			}
		}

		public string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}

		public string To
		{
			get
			{
				return this.to;
			}
			set
			{
				this.to = value;
			}
		}

		public SmtpMessage()
		{
			this.attachments = new ArrayList();
		}
	}
}