using System;
using System.Text;

namespace Utils.SMTP
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class MailMessage
	{
		public enum BodyFormat
		{
			HTML,TEXT
		}

		private string sFrom;
		private string sTo;
		private string sCc;
		private string sSubject;
		private BodyFormat bfFormat=BodyFormat.TEXT;
		private string sBody;
		private string sFromName;
		private string sToName;
        private string sFilenameAttach;

		public string Cc
		{
			get
			{
				return sCc;
			}
			set
			{
				sCc=value;
			}
		}

        public string FilenameAttach
        {
            get { return sFilenameAttach; }
            set { sFilenameAttach = value; }
        }

		public string FromName
		{
			get
			{
				if(sFromName==null || sFromName=="")
					sFromName=sFrom;

				return sFromName;
			}
			set
			{
				sFromName=value;
			}
		}

		public string ToName
		{
			get
			{
				if(sToName==null || sToName=="")
					sToName=sTo;

				return sToName;
			}
			set
			{
				sToName=value;
			}
		}

		public string From
		{
			get
			{
				return sFrom;
			}
			set
			{
				sFrom=value;
			}
		}

		public string To
		{
			get
			{
				return sTo;
			}
			set
			{
				sTo=value;
			}
		}

		public string Subject
		{
			get
			{
				return sSubject;
			}
			set
			{
				sSubject=value;
			}
		}

		public string Body
		{
			get
			{
				return sBody;
			}
			set
			{
				sBody=value;
			}
		}

		public BodyFormat MailFormat
		{
			get
			{
				return bfFormat;
			}
			set
			{
				bfFormat=value;
			}
		}

		public MailMessage()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override string ToString()
		{
			StringBuilder sb=new StringBuilder();
			sb.Append(string.Format("From: {0}\r\n",this.FromName));
			sb.Append(string.Format("To: {0}\r\n",this.ToName));
			if(this.Cc!=null && this.Cc!="")
			{
				sb.Append(string.Format("Cc: {0}\r\n",this.Cc.Replace(";",",")));
			}
			sb.Append(string.Format("Date: {0}\r\n",DateTime.Now.ToString("ddd, d M y H:m:s z")));
			sb.Append(string.Format("Subject: {0}\r\n",this.Subject));
			sb.Append(string.Format("X-Mailer: vmlinux.Net.Mail.SmtpServer\r\n"));
			switch(this.MailFormat)
			{
				case BodyFormat.TEXT:
					sb.Append(string.Format("Content-type:text/plain;Charset=GB2312\r\n"));
					break;
				case BodyFormat.HTML:
					sb.Append(string.Format("Content-type:text/html;Charset=GB2312\r\n"));
					break;
				default:
					break;
			}
            if (this.FilenameAttach == "" || System.IO.File.Exists(this.FilenameAttach) == false)
                sb.Append( string.Format("\r\n{0}\r\n", this.Body)); // Solo il body
            else
            {
                sb.Append("MIME-Version: 1.0\r\n");
                sb.Append("Content-Type: multipart/mixed;\r\n");
                sb.Append(" boundary=\"----=_NextPart_000_0032_01CE6175.E34AEF90\"\r\n");
                sb.Append("X-Priority: 3\r\n");
                sb.Append("X-MSMail-Priority: Normal\r\n");
                sb.Append("Importance: Normal\r\n");
                sb.Append("X-Mailer: Microsoft Windows Live Mail 15.4.3555.308\r\n");
                sb.Append("X-MimeOLE: Produced By Microsoft MimeOLE V15.4.3555.308\r\n");
                sb.Append("X-Spam-Rating: mxavas2.ad.aruba.it 1.6.2 0/1000/N\r\n\r\n");
                sb.Append("Messaggio multiparte in formato MIME.\r\n\r\n");
                sb.Append("------=_NextPart_000_0032_01CE6175.E34AEF90\r\n");
                sb.Append("Content-Type: multipart/alternative;\r\n");
                sb.Append("\tboundary=\"----=_NextPart_001_0033_01CE6175.E34AEF90\"\r\n\r\n\r\n");
                sb.Append("------=_NextPart_001_0033_01CE6175.E34AEF90\r\n");
                sb.Append("Content-Type: text/plain;\r\n");
                sb.Append("\tcharset=\"iso-8859-1\"\r\n");
                sb.Append("Content-Transfer-Encoding: quoted-printable\r\n\r\n");
                sb.Append(this.Body + "\r\n");
                sb.Append("------=_NextPart_001_0033_01CE6175.E34AEF90\r\n");
                sb.Append("Content-Type: text/html;\r\n");
                sb.Append("\tcharset=\"iso-8859-1\"\r\n");
                sb.Append("Content-Transfer-Encoding: quoted-printable\r\n\r\n");
                sb.Append("<HTML><HEAD></HEAD>\r\n<BODY dir=3Dltr>\r\n<DIV dir=3Dltr>\r\n<DIV style=3D\"FONT-SIZE: 12pt; FONT-FAMILY: 'Calibri'; COLOR: #000000\">\r\n");

                char[] separator = { '\r', '\n' };
                string[] stringhe = this.Body.Split(separator);

                foreach (string s in stringhe)
                {
                    sb.Append(string.Format("\r\n<DIV>{0}</DIV>", s));
                }

                string tot = "";
                using (System.IO.StreamReader reader = new System.IO.StreamReader(this.FilenameAttach))
                {
                    while (reader.EndOfStream == false)
                    {
                        string s = reader.ReadLine();

                        tot += s;
                        //sb.Append(string.Format("<DIV>{0}</DIV>", s));                      
                        tot += "\r\n";
                        //sb.Append("\r\n");

                    }
                    reader.Close();                    
                }
                sb.Append("</DIV></DIV></BODY></HTML>\r\n\r\n");
                sb.Append("------=_NextPart_001_0052_01CE6177.8E6C0750--\r\n\r\n");
                sb.Append("------=_NextPart_000_0051_01CE6177.8E6C0750\r\n");
                sb.Append("Content-Type: text/plain;\r\n");
                sb.Append(string.Format("\tname=\"{0}\"\r\n", System.IO.Path.GetFileName(this.FilenameAttach)));
                sb.Append("Content-Transfer-Encoding: 7bit\r\n");
                sb.Append("Content-Disposition: attachment;\r\n");
                sb.Append(string.Format("\tfilename=\"{0}\"\r\n", System.IO.Path.GetFileName(this.FilenameAttach)));
                sb.Append(tot);
                sb.Append("------=_NextPart_000_0051_01CE6177.8E6C0750--\r\n\r\n");
                /*
                sb.Append(" boundary=\"----=_Part_858819_31484683.1370267530918\"\r\n");
                sb.Append("X-SenderIP: 87.25.55.44\r\n");
                sb.Append("X-libjamv: 71phd1r5Pus=\r\n");
                sb.Append("X-libjamsun: ftKgPg3sNDCHUeLSSNJuodKTXmY9c0ct\r\n");
                sb.Append("X-Spam-Rating: mxavas6.ad.aruba.it 1.6.2 0/1000/N\r\n\r\n");
                sb.Append("------=_Part_858819_31484683.1370267530918\r\n");
                sb.Append("Content-Type: text/plain; charset=UTF-8\r\n");
                sb.Append("Content-Transfer-Encoding: 7bit\r\n");
                sb.Append(this.Body + "\r\n");
                sb.Append("------=_Part_858819_31484683.1370267530918\r\n");
                sb.Append(string.Format("Content-Type: text/plain; name=\"{0}\"\r\n", System.IO.Path.GetFileName(this.FilenameAttach)));
                sb.Append("Content-Transfer-Encoding: 7bit\r\n");
                sb.Append(string.Format("Content-Disposition: attachment; filename=\"{0}\"; \r\n", System.IO.Path.GetFileName(this.FilenameAttach)));
                System.IO.FileInfo fi = new System.IO.FileInfo(this.FilenameAttach);
                sb.Append(string.Format("   size={0}\r\n\r\n",fi.Length));
                string content = "";
                using(System.IO.StreamReader sr = new System.IO.StreamReader(this.FilenameAttach))
                {
                    content = sr.ReadToEnd();
                    sr.Close();                    
                }
                sb.Append(string.Format("{0}\r\n",content));
                sb.Append("------=_Part_858819_31484683.1370267530918--\r\n");
                 */
            }
            
            return sb.ToString();
		}
	}
}
