using System;
using System.IO;

namespace Utils.Email
{
	internal class SmtpAttachment
	{
		private string filename;

		public string Filename
		{
			get
			{
				return this.filename;
			}
		}

		public SmtpAttachment(string filename)
		{
			this.filename = filename;
			this.CheckFile(filename);
		}

		private void CheckFile(string filename)
		{
			try
			{
				File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read).Close();
			}
			catch
			{
				throw new ArgumentException("Bad attachment", filename);
			}
		}
	}
}