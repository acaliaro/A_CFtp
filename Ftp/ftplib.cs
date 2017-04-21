using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Diagnostics;

#if __ANDROID__
#else
using System.Windows.Forms;
#endif
namespace Utils.Ftp
{
    /// <summary>
    /// Raised when a string is sent to / received from the server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FtpMessagesEventHandler(object sender, FtpMessagesEventArgs e);

    /// <summary>
    /// Raised when,during a Download or Upload action, the file transfer percentage is changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FtpTransferPercentageEventHandler(object sender, FtpTransferPercentageEventArgs e);

 	public class FtpList
	{
		private string filePermission = "";
		private int counter = 0;
		private string owner = "";
		private string group = "";
		private int size = 0;
		private string modificationDate = "";
		private string fileName = "";
		private bool isDirectory = false;

		public FtpList()
		{
		}

		public string FilePermission
		{
			get { return filePermission; }
			set { filePermission = value; }
		}
		public int Counter
		{
			get { return counter; }
			set { counter = value; }
		}
		public string Owner
		{
			get { return owner; }
			set { owner = value; }
		}
		public string Group
		{
			get { return group; }
			set { group = value; }
		}
		public int Size
		{
			get { return size; }
			set { size = value; }
		}
		public string ModificationDate
		{
			get { return modificationDate; }
			set { modificationDate = value; }
		}
		public string FileName
		{
			get { return fileName; }
			set { fileName = value; }
		}
		public bool IsDirectory
		{
			get { return isDirectory; }
			set { isDirectory = value; }
		}

		public void Fill(string stringaDaList, Utils.Ftp.Ftp.EnumSyst syst)
		{
			int pos = 0;
			int count = 1;
			string stringa = "";

            Ftp.EnumSyst tipoVisualizzazione;
            if (syst == Ftp.EnumSyst.Unix ||
                (syst == Ftp.EnumSyst.Windows && (stringaDaList[0] == 'd' || stringaDaList[0] == '-')))
                tipoVisualizzazione = Ftp.EnumSyst.Unix;
            else
                tipoVisualizzazione = Ftp.EnumSyst.Windows;


			while (pos < stringaDaList.Length)
			{
                // C'è un problema con i sistemi Windows in quanto IIS può gestire sia la visualizzazione DOS che UNIX e non c'è un comando che ti dica che tipo di 
                // visualizzazione è attualmente attiva. allora controllo il primo carattere della stringa...
				if (tipoVisualizzazione == Ftp.EnumSyst.Unix)
				{
					// Controllo se è una directory
					if (pos == 0 && stringaDaList[0] == 'd')
						isDirectory = true;
					if (stringaDaList[pos] == ' ')
					{
						if (stringa != "")
						{
							if (count == 1)
								this.filePermission = stringa;
							else if (count == 2)
								this.counter = System.Convert.ToInt32(stringa);
							else if (count == 3)
								this.owner = stringa;
							else if (count == 4)
								this.group = stringa;
							else if (count == 5)
								this.size = System.Convert.ToInt32(stringa);
							else if (count == 6)
							{
								this.modificationDate = stringa;
								// la data è un insieme di stringhe separate da spazi. 
								// dovrebbe essere di lunghezza = 12;
								this.modificationDate += stringaDaList.Substring(pos, 12 - this.modificationDate.Length);
								pos += 12 - stringa.Length;

                                // E me ne esco dal loop per gestire il filename
                                break;
							}

							count++; // incremento l'indice del campo
						}

						stringa = "";
					}
					else
						stringa += stringaDaList[pos].ToString();

					pos++;
				}
                else if (tipoVisualizzazione == Ftp.EnumSyst.Windows)
                {
                    if (stringaDaList[pos] == ' ')
                    {
                        if (stringa != "")
                        {
                            if (count == 1)
                                this.ModificationDate = stringa;
                            else if (count == 2)
                                this.ModificationDate += " " + stringa;
                            else if (count == 3)
                            {
                                // O trovo DIR, o non trovo nulla fino al campo successivo
                                if (stringa == "<DIR>")
                                {
                                    this.isDirectory = true;
                                    count++;
                                    break;
                                }
                                else
                                    count++;
                            }

                            if (count == 4)
                            {
                                this.size = System.Convert.ToInt32(stringa);

                                // Gestisco il filename, ultimo campo
                                break;
                            }

                            count++; // incremento l'indice del campo
                        }

                        stringa = "";
                    }
                    else
                        stringa += stringaDaList[pos].ToString();

                    pos++;
                }
			}

                
            if(((tipoVisualizzazione == Ftp.EnumSyst.Unix) && count == 6) ||
                (tipoVisualizzazione == Ftp.EnumSyst.Windows && count == 4))
            {

                // Se sono sull'ultimo campo, prendo il filename

                // A questo punto, quello che c'è dopo è il nome del file. Mi sposto in avanti di una posizione
                // in quanto c'è comunque uno spazio
                pos++;

                // Poi prendo tutto il resto
                this.fileName = stringaDaList.Substring(pos, stringaDaList.Length - pos).Trim();


            }

		}
	}

    public class FtpMessagesEventArgs : EventArgs
    {
        private string message;
        public FtpMessagesEventArgs(string message)
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

    /// <summary>
    /// Raised when, during a Download or Upload action, the file transfer percentage change
    /// </summary>
    public class FtpTransferPercentageEventArgs : EventArgs
    {
        public enum EnumTransferType { Download, Upload };

        private string _filename;
        private EnumTransferType _transferType;
        private int _percentage;
        private long _byteTransferred;
        private long _fileSize;

        public FtpTransferPercentageEventArgs(string filename, EnumTransferType transferType,  int percentage, long byteTransferred, long fileSize)
        {
            _filename = filename;
            _transferType = transferType;
            _percentage = percentage;
            _byteTransferred = ByteTransferred;
            _fileSize = fileSize;
        }
        public string Filename
        {
            get { return _filename; }
        }
        public EnumTransferType TransferType
        {
            get { return _transferType; }
        }
        public int Percentage
        {
            get { return _percentage; }
        }
        public long ByteTransferred
        {
            get { return _byteTransferred; }
        }
        public long FileSize
        {
            get { return _fileSize; }
        }
    }

	public class Ftp
	{
        public event FtpMessagesEventHandler FtpMessages;
        public event FtpTransferPercentageEventHandler FtpTransferPercentage;

        
		public const string RELEASE = "2.14.1";
		public const string DATARELEASE = "24/10/2013";

        /*
         * release 2.14.1 del 24/10/2013
         * - modificata visualizzazione dei dati iniziali
         * release 2.14.0 del 06/06/2013
         * - spostata gestione logger in apposita classe
         * release 2.13.7 del 30/04/2013
         * - in _ftpServer tolta stringa iniziale ftp://
         * release 2.13.6 del 31/07/2012
         * - inserita proprietà VisualConnectionParameters
         * release 2.13.5 del 30/07/2012
         * - inserita readResponse(); nella disconnect così da ricevere il messaggio di saluto dall'ftpserver
         * release 2.13.4 del 03/05/2012
         * - risolto problema di lentezza nel download inserendo closeDataSocket nella doDownload()
         * - messo while al posto dell'if nella sendcommand, quando svuoto il _mainSocket
         * release 2.13.3 del 02/05/2012
         * - tolta chiusura del file e del socket in doDownload e del solo file in doUpload
         * - inserita chiusura del file nel closeDataSocket
         * - inserito svuotamento del _mainSocket in sendCommand
         * - modificato controllo if(_bytesTotal >= _fileSize && _dataSock.Available == 0) in doDownload
         * release 2.13.2 del 23/09/2011
         * - ulteriore problema sul 226, questa volta relativo al comando RETR in filezilla...
         * release 2.13.1 del 22/09/2011
         * - ricorretto problema su ritorno 226 nei NLST command. In poche parole, se il comando torna subito 226, al termine della
         *   lista non rimango in attesa di un comando di chiusura
         * release 2.13.0 del 21/09/2011
         * - inserita proprietà LogDirectoryName per poter modificare la directory dove risiede il file di log...
         * release 2.12.0 del 26/08/2011
         * - inserita nuova gestione del file di log parametrizzando il numero di giorni di log (0 = gestione attuale con cancellazione del file ad ogni connect)
         * release 2.11.2 del 10/07/2011
         * - corretta gestione di ExistFile e List in quanto il comando NLST torna 226 command terminated in momenti diversi a seconda che siamo su unix o windows
         * - corretta gestione List.fill in quanto in windows, la visualizzazione dei dati può essere fatta in formato unix o windows
         * release 2.11.1 del 08/07/2011
         * - in ExistFile() diviso i comandi tra unix e windows in quanto l'ntfs ed il size danno dei problemi a seconda
         *   dei sistemi operativi
         * release 2.11.0 del 08/07/2011
         * - nella openDownload tolto trycatch sulla getFileSize, così che se non riesce a prendere la dimensione del file remoto,
         *   il programma va in exception
         * release 2.10.0 del 05/07/2011
         * - inserito controllo in ExistsFile per "no such file or directory" error (unix / finiper)
         * release 2.9.5 del 17/06/2011
         * - moltiplicato timeout * 1000 se minore di 100
         * release 2.9.4 del 10/06/2011
         * - corretto problema in createConfigFileName
         * release 2.9.3 del 06/03/2011
         * - inseriti parametri connessione RAS nel config
         * release 2.9.2 del 22/02/2011
         * - inserito un controllo nella funzione doGetHostEntryAsync per verificare il tipo di rete ritornata
         * release 2.9.0 del 08/02/2011
         * - inserito evento FtpTransferPercentage che ritorna la percentuale di trasferimento
         * release 2.8.0 del 11/11/2010
         * - corretta "list" per i filename contententi spazi
         * release 2.7.1 del 11/11/2010
         * - implementato metodo ExistFile che utilizza il comando NLST
         * - inserita chiamata a ExistFile in List per prevenire il problema della lentezza qualora si richieda la list di file non presenti
         * release 2.7.0 del 09/11/2010
         * - gestito il download di un file a "0" byte
         * - inserito controllo su "no such file or directory" in LIST
         * release 2.6.0 del 08/11/2010
         * - Inserito comando Pwd() da utilizzare solo per test
         * - inserita funzione checkConnection()
         * release 2.5.7 del 29/10/2010
         * - eliminata dalla close la "fail" in quanto generava un exception in caso di errore
         * release 2.5.6 del 24/09/2010
         * - in doGetHostEntryAsync modificata la chiamata a GetHostEntryFinished.WaitOne
         * - in doGetHostEntryAsync inserita chiamata a Dns.GetHostEntry
         * - tryCatch in getHostEntryCallback
         * release 2.5.5 del 11/09/2010
         * - eliminata la proprietà emailAddress sostituita con CreatedBy
         * release 2.5.4 del 10/09/2010
         * - creata e resa pubblica proprietà emailAddres
         * release 2.5.3 del 04/07/2010
         * - resa pubblica proprietà fileSize
         * release 2.5.2 del 22/03/2010
         * - corretto problema nell'opensocket quando sono collegato ad un pc con activesync e 
         *   PASV mi torna 127.0.0.1
         * release 2.5.1 del 13/03/2010
         * - inserita gestione saveFtpServer
         * - inserito metodo DefaultFtpPort
         * release 2.5.0 del 04/03/2010
         * - modificati nomi ai parametri server, user
         * - inseriti metodi WriteConfigFile e ReadConfigFile con gestione XML
         * - eliminato metodo readConfigFile
         * - reso pubblica proprietà sleeptime
         * - inserito metodo ExistConfigFile per verificare esistena file di config
         * release 2.4.1 del 22/02/2010
         * - inserita gestione DEMO
         * release 2.4.0 del 09/02/2010
         * - inserito evento per la visualizzazione dei messaggi
         * - rese pubbliche le proprietà ftpServer, userName, password, port
         * - inserito costruttore vuoto
         * release 2.3.0 del 08/02/2010
         * - modificata Connect con la gestione oltre che dell'ip, anche del nome server per esteso
         * - eliminata la funzione setBinary
         * - modificata proprietà Testo in TextBox
         * - inserita funzione Type
         * - inserito enumType
         * release 2.2.4 del 12/01/2010
         * - modificate voci da italiano in inglese
         * release 2.2.3 del 22/12/2009
         * - inserita funzione isTimeout in download
         * release 2.2.2 del 19/12/2009
         * - eliminati i vari "connect()" dalle varie funzioni ftp. Occorre chiamare esplicitamente il metodo connect()
         * - inserita cancellazione del "testo" all'inizializzazione della classe ftp
         * - gestita list per msdos
         * release 2.2.1 del 12/12/2009
         * - spostata classe FtpCommands in A_CFormFtp.dll
         * release 2.2.0 del 11/12/2009
         * - inserita classe FtpCommands Utilizzata dalla classe FormFtp
         * release 2.1.0 del 25/11/2009
         *	- inserite istruzioni IpAddress.parse nella creazione dei socket
         * release 2.0.1 del 24/11/2009
         *	- modificata visualizzazione al connect
         * release 2.0.0 del 23/11/2009
         *	-inserita parametrizzazione timeout, sleep, bytesArray in ftpconfig
         */

		#if __ANDROID__
		#else
        private Utils.Logger _logger = null;
		#endif
        public enum EnumSyst{Undefined, Windows, Unix};
        public enum EnumType { Ascii, Ebcdic, Image};

		private const int DATA_CONNECTION_ALREADY_OPEN = 125;
		private const int FILE_STATUS_OK = 150;
		private const int FILE_STATUS = 213;
		private const int SYSTEM_TYPE = 215;
		private const int SERVICE_READY_FOR_NEW_USER = 220;
		private const int CLOSING_DATA_CONNECTION = 226;
        private const int TRANSFER_OK = 226;
		private const int ENTERING_PASSIVE_MODE = 227;
		private const int USER_LOGGED_IN = 230;
        private const int SECURETY_DATA_EXCHANGE_COMPLETE = 234;
		private const int REQUESTED_FILE_ACTION_OK = 250;
		private const int PATHNAME_CREATED = 257;
		private const int USERNAME_OK_NEED_PASSWORD = 331;
		private const int REQUESTED_FILE_ACTION_PENDING = 350;
        private const int NO_SUCH_FILE = 450;
		private const int COMMAND_UNRECOGNIZED = 500;
        private const int SIZE_NOT_ALLOWED_IN_ASCII_MODE = 550;
        private const int DIRECTORY_NOT_FOUND = 550;

        //private string CONFIG_FILE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "") + "\\ftpconfig.xml";
		private const int DEFAULT_PORT = 21;
		private const int DEFAULT_TIMEOUT = 10000; // timeout in millisseconds
		private const int DEFAULT_SLEEP_TIME = 100; // Millisecond di sleep
		private const int DEFAULT_BYTE_ARRAY_SIZE = 512; // Dimensione dell'array di byte 

        private bool _useSsl = false;
        private bool _visualConnectionParameters = true;
        private EnumSyst _syst = EnumSyst.Undefined;
        private string _ftpServer = "";
		private string _userName="";
		private string _password="";
		private int _port = DEFAULT_PORT;
		private int _timeout = DEFAULT_TIMEOUT; // timeout in miliseconds
		private int _sleepTime = DEFAULT_SLEEP_TIME;
		private int _byteArraySize = DEFAULT_BYTE_ARRAY_SIZE;
		private long _bytesTotal = 0; // upload/download info if the user wants it.
		private long _fileSize; // gets set when an upload or download takes place
		private string _responseStr; // server response if the user wants it.
		private string _messages = ""; // server messages
        private string _createdBy = "info@mobi-ware.it";
        private string _configDirectoryName = "";
        private bool _viewAllInitData = true;

        private string _rasEntry = "";
        private string _rasUserName = "";
        private string _rasPassword = "";

		private Socket _mainSock = null;
		private IPEndPoint _mainIpEndPoint = null;
		private Socket _dataSock = null;
		private IPEndPoint _dataIpEndPoint = null;
		private FileStream _file = null;
		private int _response;
		private string _bucket = "";
#if __ANDROID__
#else
		public Utils.Logger Logger
		{
		get { return _logger; }
		set { _logger = value; }
		}
#endif
		private string _configFile = "";
		private Byte[] _bytesArray = null;
        private string _string1 = "";
        private string _string2 = "";


				
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ftpServer"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="port"></param>
		public Ftp(string ftpServer, string userName, string password, int port)
		{
			this._ftpServer = ftpServer;
			this._userName = userName;
			this._password = password;
			this._port = port;
            this._configFile = configFile() ;
			#if __ANDROID__
			#else
            initLogger();
			#endif
		}
		#if __ANDROID__
		#else
        private void initLogger()
        {
            int saveDaysOfLog = 0;
            if (_logger != null) // In precedenza ho già instanziato la variabile o ho letto l'xml con i dati di configurazione
                saveDaysOfLog = _logger.DaysOfLog;
            _logger = new Logger("ftplog{0}.txt", "ftplog*.txt");
            _logger.DaysOfLog = saveDaysOfLog;
        }
		#endif
        /// <summary>
        /// Contructor
        /// </summary>
        public Ftp()
        {
			#if __ANDROID__
			#else
			initLogger();
			#endif
		}

        /// <summary>
        /// Return default Ftp port
        /// </summary>
        /// <returns>Default ftp port</returns>
        public static int DefaultFtpPort()
        {
            return DEFAULT_PORT;
        }

        public bool UseSsl
        {
            get { return _useSsl; }
            set { _useSsl = value; }
        }

        private string configFile()
        {
#if __ANDROID__
			return ""; // Per ora non gestita
            //return Utils.ClassUtils.FileInApplicationDir("ftpconfig.xml");
            //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ftpconfig.xml");
#else
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "") + "\\ftpconfig.xml";
#endif
        }


        /// <summary>
        /// Read a default XML config file and fill properties
        /// </summary>
        public void ReadConfigFile()
        {
            ReadConfigFile("");
        }

        /// <summary>
        /// Read a XML config file with modified name, and fill properties
        /// </summary>
        /// <param name="prefix">String added to filename</param>
        public void ReadConfigFile(string prefix)
        {
            //System.Xml.XmlTextReader xmlTextReader = null;
            try
            {
                string xmlFileName = createConfigFileName(prefix);
                string element = "";
                if (ExistConfigFile(prefix) == false)
                    fail("FTP Config file is not present");

                using (System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(xmlFileName))
                {
                    while (xmlTextReader.Read())
                    {
                        if (xmlTextReader.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            element = xmlTextReader.Name.ToUpper();
                            if (element == "FTP")
                                continue;

                            xmlTextReader.Read();
                            if (xmlTextReader.NodeType == System.Xml.XmlNodeType.Text)
                            {
                                switch (element)
                                {
                                    case "FTPSERVER":
                                        this._ftpServer = xmlTextReader.Value;
                                        break;
                                    case "USERNAME":
                                        this._userName = xmlTextReader.Value;
                                        break;
                                    case "PASSWORD":
                                        this._password = xmlTextReader.Value;
                                        break;
                                    case "PORT":
                                        this._port = System.Convert.ToInt32(xmlTextReader.Value);
                                        break;
                                    case "TIMEOUT":
                                        this._timeout = System.Convert.ToInt32(xmlTextReader.Value);
                                        if (this._timeout < 100)
                                            this._timeout *= 1000; // Lo porto in millisecondi
                                        break;
                                    case "SLEEPTIME":
                                        this._sleepTime = System.Convert.ToInt32(xmlTextReader.Value);
                                        break;
                                    case "STRING1":
                                        this._string1 = xmlTextReader.Value;
                                        break;
                                    case "STRING2":
                                        this._string2 = xmlTextReader.Value;
                                        break;
                                    case "RASENTRY":
                                        this._rasEntry = xmlTextReader.Value;
                                        break;
                                    case "RASUSERNAME":
                                        this._rasUserName = xmlTextReader.Value;
                                        break;
                                    case "RASPASSWORD":
                                        this._rasPassword = xmlTextReader.Value;
                                        break;
                                    case "DAYSOFLOG":
									#if __ANDROID__
									#else
                                        if (_logger == null)
                                            _logger = new Logger(); // Istanzio la proprietà
                                        _logger.DaysOfLog = System.Convert.ToInt32(xmlTextReader.Value);
									#endif
                                        break;
                                    //case "EMAILADDRES":
                                    //    if (xmlTextReader.Value.Trim() != "")
                                    //        this.emailAddres = xmlTextReader.Value;
                                    //    break;
                                }
                            }
                        }
                    }

                    xmlTextReader.Close();
                }
            }
            catch (Exception ex)
            {
                fail(ex.Message);
            }
            finally
            {
                //if (xmlTextReader != null)
                //    xmlTextReader.Close();
#if DEMO
                doMessageBox("'READCONFIGFILE' DONE");
#endif
            }
        }

        /// <summary>
        /// Verify if default config file exist
        /// </summary>
        /// <returns>true = exist</returns>
        public bool ExistConfigFile()
        {
            return ExistConfigFile("");
        }

        /// <summary>
        /// Verify in config file with new name exist
        /// </summary>
        /// <param name="prefix">String added to filename</param>
        /// <returns>true = exist</returns>
        public bool ExistConfigFile(string prefix)
        {
            return System.IO.File.Exists(createConfigFileName(prefix));
        }

        /// <summary>
        /// Write properties to a XML default file
        /// </summary>
        public void WriteConfigFile()
        {
            WriteConfigFile("");
        }

        /// <summary>
        /// Write properties to a XML file with modified name
        /// </summary>
        /// <param name="prefix">String added to default filename</param>
        public void WriteConfigFile(string prefix)
        {
            //System.Xml.XmlTextWriter xmlTextWriter = null;
            try
            {
                string xmlFileName = createConfigFileName(prefix);
				#if __ANDROID__
				#else
				if (_logger == null)
                    _logger = new Logger();
				#endif
                using (System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(xmlFileName, null))
                {
                    xmlTextWriter.WriteRaw("<Ftp>\n" +
                    "    <FtpServer>" + this._ftpServer + "</FtpServer>\n" +
                    "    <Username>" + this._userName + "</Username>\n" +
                    "    <Password>" + this._password + "</Password>\n" +
                    "    <Port>" + this._port.ToString() + "</Port>\n" +
                    "    <Timeout>" + this._timeout.ToString() + "</Timeout>\n" +
                    "    <SleepTime>" + this._sleepTime.ToString() + "</SleepTime>\n" +
                    "    <String1>" + this._string1 + "</String1>\n" +
                    "    <String2>" + this._string2 + "</String2>\n" +
                    "    <RasEntry>" + this._rasEntry + "</RasEntry>\n" +
                    "    <RasUsername>" + this._rasUserName + "</RasUsername>\n" +
                    "    <RasPassword>" + this._rasPassword + "</RasPassword>\n" +
						#if __ANDROID__
						#else
                    "    <DaysOfLog>" + _logger.DaysOfLog + "</DaysOfLog>\n" +
						#endif
                    //"    <EmailAddres>" + this.emailAddres + "</EmailAddres>\n" +
                    "  </Ftp>\n");
                    xmlTextWriter.Close();
                }

            }
            catch (Exception ex)
            {
                fail(ex.Message);
            }
            finally
            {
                //if (xmlTextWriter != null)
                //    xmlTextWriter.Close();
#if DEMO
                doMessageBox("'WRITECONFIGFILE' DONE");
#endif
            }
        }

        public string ConfigDirectoryName
        {
            get
            {
                // Se ho definito un nome di directory diverso da quello di default, lo ritorno
                if (_configDirectoryName != "")
                {
                    if (System.IO.Directory.Exists(_configDirectoryName))
                        return _configDirectoryName + System.IO.Path.DirectorySeparatorChar;
                }

                return System.IO.Path.GetDirectoryName(configFile()) + System.IO.Path.DirectorySeparatorChar;
            }
            set { _configDirectoryName = value; }
        }


        private string createConfigFileName(string prefix)
        {
            string xmlFileName;

            string configFileName = ConfigDirectoryName + System.IO.Path.GetFileName(configFile());

            if (prefix.Trim() != "")
            {
                // E' una soluzione temporanea per permettere di gestire un nome di file xml invece che il prefix
                if (System.IO.Path.IsPathRooted(prefix))
                    return prefix;

                xmlFileName = System.IO.Path.GetFileName(configFileName);
                xmlFileName = prefix + "_" + xmlFileName;
                xmlFileName = System.IO.Path.GetDirectoryName(configFileName) + System.IO.Path.DirectorySeparatorChar + xmlFileName;
            }
            else
                xmlFileName = configFileName;

            return xmlFileName;
 

            //if (prefix.Trim() != "")
            //{
            //    // E' una soluzione temporanea per permettere di gestire un nome di file xml invece che il prefix
            //    if (System.IO.Path.IsPathRooted(prefix))
            //        return prefix;

            //    xmlFileName = System.IO.Path.GetFileName(CONFIG_FILE);
            //    xmlFileName = prefix + "_" + xmlFileName;
            //    xmlFileName = System.IO.Path.GetDirectoryName(CONFIG_FILE) + "\\" + xmlFileName;
            //}
            //else
            //    xmlFileName = CONFIG_FILE;
            //return xmlFileName;
        }
		/// <summary>
		/// Verify connection status
		/// </summary>
		/// <returns>true = connected, false = not connected</returns>
		public bool IsConnected()
		{
			#if __ANDROID__
			#else
			string s = "SONO IN: IsConnected";
			_logger.WriteLogFile(s);            
			#endif
			if (_mainSock != null)
				return _mainSock.Connected;
			else
				return false;
		}

		private void fail(string error)
		{
            try
            {
				#if __ANDROID__
				#else
				string s = "SONO IN: fail: " + error;
				_logger.WriteLogFile(s);
				#endif
            }
            catch { }
			throw new Exception(error);
		}

		private void fail()
		{
            try
            {
				#if __ANDROID__
				#else
				string s = "SONO IN: fail: " + _responseStr;
                _logger.WriteLogFile(s);
				#endif
            }
            catch { }
			throw new Exception(_responseStr);
		}

#if __ANDROID__
#else
		/// <summary>
		/// TextBox where FTP messages are written to
		/// </summary>
        //public System.Windows.Forms.TextBox TextBox
        //{
        //    get
        //    {
        //        return this._testo;
        //    }
        //    set
        //    {
        //        this._testo = value;
        //        if (_testo.Multiline == false)
        //            _testo.Multiline = true;
        //    }
        //}
#endif
        /// <summary>
        /// Do TYPE Command
        /// </summary>
        /// <param name="type">Type value</param>
        public void Type(EnumType type)
        {
			#if __ANDROID__
			#else
            string s = "SONO IN: Type";
            _logger.WriteLogFile(s);
			#endif

            if (checkConnection() == false)
                return;

            switch(type)
            {
                case EnumType.Ascii:
                    sendCommand("TYPE A");
                    break;
                case EnumType.Ebcdic: 
                    sendCommand("TYPE E");
                    break;
                case EnumType.Image:
                    sendCommand("TYPE I");
                    break;
            }

            readResponse();
            if (_response != 200)
                fail();
        }

        private void sendCommand(string command)
        {
            sendCommand(command, true);
        }

		private void sendCommand(string command, bool writeToLog)
		{
			#if __ANDROID__
			string s  = "";
			#else
            string s = "SONO IN: sendCommand";
            _logger.WriteLogFile(s);
			#endif
			Byte[] cmd = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());

            if (writeToLog)
            {
                s = command;
				#if __ANDROID__
				#else
                _logger.WriteLogFile(s);
				#endif
                OnTextReceived(s);
            }
            while(_mainSock.Available > 0) // Serve per pulire il buffer prima di inviare un nuovo comando...
                _mainSock.Receive(_bytesArray, _bytesArray.Length, 0);

			_mainSock.Send(cmd, cmd.Length, 0);
		}

		private void fillBucket()
		{
			#if __ANDROID__
			#else
			string s = "SONO IN: fillBucket";
            _logger.WriteLogFile(s);
			#endif
			long bytesgot;

            if(isTimeout(_mainSock))
                fail("Timeout from server");

			while(_mainSock.Available > 0)
			{
				bytesgot = _mainSock.Receive(_bytesArray, _bytesArray.Length, 0);
				_bucket += Encoding.ASCII.GetString(_bytesArray, 0, (int)bytesgot);
				// this may not be needed, gives any more data that hasn't arrived
				// just yet a small chance to get there.
				System.Threading.Thread.Sleep(_sleepTime);
			}
		}

		private string getLineFromBucket()
		{
			#if __ANDROID__
			#else
			string s = "SONO IN: getLineFromBucket";
            _logger.WriteLogFile(s);
			#endif
			int i;
			string buf = "";

			if((i = _bucket.IndexOf('\n')) < 0)
			{
				while(i < 0)
				{
					fillBucket();
					i = _bucket.LastIndexOf('\n'); // ??? Prova
				}
			}

            // Cerco di risolvere un problema dato da alcuni server che ritornano + codici nella stessa stringa
            int prevLF = _bucket.Substring(0, i).LastIndexOf('\n');
            if (prevLF >= 0)
            {
                _bucket = _bucket.Substring(prevLF + 1, i - prevLF);
                i = _bucket.LastIndexOf('\n');
            }

			buf = _bucket.Substring(0, i);
			_bucket = _bucket.Substring(i + 1);

			#if __ANDROID__
			#else
            s = "buf=" + buf + ", bucket=" + _bucket;
            _logger.WriteLogFile(s);
			#endif
            //OnTextReceived(s);
            return buf;
		}

        /// <summary>
        /// Send a command to the ftp server and receive the response. Only for test
        /// </summary>
        /// <param name="command">Command to send</param>
        public void SendCommand(string command)
        {
            sendCommand(command);
            readResponse();
        }

        private void readResponse()
        {
            readResponse(true);
        }

		// Any time a command is sent, use ReadResponse() to get the response
		// from the server. The variable responseStr holds the entire string and
		// the variable response holds the response number.
		private void readResponse(bool writeToLog)
		{
			string buf;
			_messages = "";

            string s = "SONO IN: readResponse";
			#if __ANDROID__
			#else
            _logger.WriteLogFile(s);
			#endif
			while(true)
			{
				buf = getLineFromBucket();
                if (writeToLog)
                {
                    s = buf;
                    _logger.WriteLogFile(s);
                    OnTextReceived(s);
                }
				// the server will respond with "000-Foo bar" on multi line responses
				// "000 Foo bar" would be the last line it sent for that response.
				// Better example:
				// "000-This is a multiline response"
				// "000-Foo bar"
				// "000 This is the end of the response"
				if(Regex.Match(buf, "^[0-9]+ ").Success)
				{
					_responseStr = buf;
					_response = int.Parse(buf.Substring(0, 3));
					break;
				}
				else
					_messages += Regex.Replace(buf, "^[0-9]+-", "") + "\n";
			}
		}

		// if you add code that needs a data socket, i.e. a PASV command required,
		// call this function to do the dirty work. It sends the PASV command,
		// parses out the port and ip info and opens the appropriate data socket
		// for you. The socket variable is private Socket dataSocket. Once you
		// are done with it, be sure to call CloseDataSocket()
		private void openDataSocket()
		{
            string s = "SONO IN: openDataSocket";
            _logger.WriteLogFile(s);
            
            string[] pasv = null;
			int myPort;
            string myFtpServer = "";

			sendCommand("PASV");
			readResponse();
			if(_response != ENTERING_PASSIVE_MODE)
				fail();

			try
			{
				int i1, i2;

				i1 = _responseStr.IndexOf('(') + 1;
				i2 = _responseStr.IndexOf(')') - i1;
                pasv = _responseStr.Substring(i1, i2).Split(',');
			}
			catch(Exception ex)
			{
				fail(ex.Message);
			}

			if(pasv.Length < 6)
				fail();

			// Caliaro per CF
//			ftpServer = String.Format("{0}.{1}.{2}.{3}", pasv[0], pasv[1], pasv[2], pasv[3]);
			myFtpServer = pasv[0] + "." + pasv[1] + "." + pasv[2] + "." + pasv[3];

            myPort = (int.Parse(pasv[4]) << 8) + int.Parse(pasv[5]);            

			try
			{
                s = "Data socket: " + myFtpServer + ":" + myPort.ToString();
                _logger.WriteLogFile(s);
                OnTextReceived(s);

				if(_dataSock != null)
				{
                    if (_dataSock.Connected)
                    {
                        //_dataSock.Shutdown(SocketShutdown.Both); // PROVA
                        _dataSock.Close();
                    }

					_dataSock = null;
				}

				if(_dataIpEndPoint != null)
					_dataIpEndPoint = null;

                s = "Creating socket...";
                _logger.WriteLogFile(s);
                OnTextReceived(s);
                _dataSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //LingerOption lo = new LingerOption(false, 0);
                //_dataSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lo);

                s = "Resolving host";
                _logger.WriteLogFile(s);
                OnTextReceived(s);
                //dataIpEndPoint = new IPEndPoint(Dns.GetHostByName(myFtpServer).AddressList[0], myPort);

                /*
                 * Se mi connetto con activesyn e cerco di traserire dei dati direttamente sul PC al quale sono
                 * collegato, il comando PASV mi torna 127.0.0.1 che poi non posso utilizzarlo nella funzione IPAddress.Parse.
                 * Allora in questo caso utilizzo l'indirizzo dell'ftp server originale
                 */
                if (myFtpServer == "127.0.0.1")
                    myFtpServer = this._ftpServer;
                    
                    
                try
                {
                    _dataIpEndPoint = new IPEndPoint(IPAddress.Parse(myFtpServer), myPort);
                }
                catch
                {
                    _dataIpEndPoint = new IPEndPoint(Dns.GetHostEntry(myFtpServer).AddressList[0], myPort);
                }
                //dataIpEndPoint = doGetHostEntryAsync(ftpServer);

                s = "Connecting..";
                _logger.WriteLogFile(s);
                OnTextReceived(s);
                _dataSock.Connect(_dataIpEndPoint);

                s = "Connected.";
                _logger.WriteLogFile(s);
                OnTextReceived(s);

			}
			catch(Exception ex)
			{
				fail(ex.Message);
			}
		}

		private void closeDataSocket()
		{
            string s = "SONO IN: closeDataSocket";
            _logger.WriteLogFile(s);

            if (_file != null)
            {
                _file.Close();
                _file = null;
            }

			if(_dataSock != null)
			{
                s = "Attempting to close data channel socket...";
                _logger.WriteLogFile(s);
                OnTextReceived(s);
                s = "Closing data channel socket!";
                _logger.WriteLogFile(s);
                OnTextReceived(s);
                //_dataSock.Shutdown(SocketShutdown.Both); // PROVA
                _dataSock.Close();

                s = "Data channel socket closed!";
                _logger.WriteLogFile(s);
                OnTextReceived(s);
                _dataSock = null;
			}

			_dataIpEndPoint = null;
		}


		private void disconnect()
		{
            string s = "SONO IN: disconnect";
            _logger.WriteLogFile(s);

            //closeDataSocket();

			if(_mainSock != null)
			{
				if(_mainSock.Connected)
				{
					sendCommand("QUIT");
                    readResponse();
					_mainSock.Close();
				}
				_mainSock = null;
			}

			if(_file != null)
				_file.Close();

			_mainIpEndPoint = null;
            _dataIpEndPoint = null;
			_file = null;
		}

		/// <summary>
		/// Do the FTP connection
		/// </summary>
		public void Connect()
		{
            //_logger = new Logger("\\my documents\\filename.txt","");
            _logger.Start();

            string s = "SONO IN: connect";
            _logger.WriteLogFile(s);

            try
            {
                if (IsConnected())
                    Close();
            }
            catch (Exception ex)
            {
                s = "Errore in close:" + ex.Message;
                _logger.WriteLogFile(s);

            }

            // Creo il bytearray
            _bytesArray = new byte[this._byteArraySize];
            string stringa = "FtpLib release:" + RELEASE + " - " + DATARELEASE + "\r\n";
            if(CreatedBy != "")
                stringa += "Created by " + CreatedBy + "\r\n";
            stringa += "Server:" + this._ftpServer + "\r\nUser:" + _userName + "\r\n";
            if(_viewAllInitData)
                stringa += "Timeout milliseconds = " + this._timeout.ToString() + "\r\nSleep milliseconds = " + this._sleepTime.ToString() + "\r\nPacket size = " + _byteArraySize.ToString() + "\r\n";

            if (_visualConnectionParameters)
            {
                s = stringa;
                _logger.WriteLogFile(s);
                OnTextReceived(s);

            }
            if (this._ftpServer.Trim() == "")
                throw new Exception("FtpServer undefined.");
            this._ftpServer = this._ftpServer.Replace("ftp://", "");
            if (_visualConnectionParameters)
            {
                s = "Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)";
                _logger.WriteLogFile(s);
            }
            _mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _mainIpEndPoint = doGetHostEntryAsync();
            }
            catch (Exception ex)
            {
                fail(ex.Message);
                return;
            }

            try
            {
                if (_visualConnectionParameters)
                {
                    s = "mainSock.Connect(mainIpEndPoint) " + _mainIpEndPoint.Address.GetAddressBytes().ToString();
                    _logger.WriteLogFile(s);

                }

                // TEST SSL
                if (_useSsl)
                {
                    using (SslHelper sshelper = new SslHelper(_mainSock, _ftpServer, true))
                    {
                        _mainSock.Connect(_mainIpEndPoint);
                        readResponse(_visualConnectionParameters);
                        if (_response != SERVICE_READY_FOR_NEW_USER)
                            fail();
                        sendCommand("AUTH SSL");
                        readResponse(_visualConnectionParameters);
                        if (_response != SECURETY_DATA_EXCHANGE_COMPLETE)
                            fail();
                        //sshelper.DoHandshake(_mainSock);
                        
                    }
                }
                // FINE TEST SSL
                else
                    _mainSock.Connect(_mainIpEndPoint);
            }
            catch (Exception ex)
            {
                fail("Error connecting to server. " + ex.Message + " - Verify TCP / IP Connection.");
            }

            if (!_useSsl)
            {
                // Per l'ssl questo controllo l'ho già fatto
                readResponse(_visualConnectionParameters);
                if (_response != SERVICE_READY_FOR_NEW_USER)
                    fail();
            }
            
            sendCommand("USER " + _userName, false);
            readResponse(_visualConnectionParameters);

            switch (_response)
            {
                case USERNAME_OK_NEED_PASSWORD:
                    if (_password.Trim() == "")
                        fail("Password undefined");
                    sendCommand("PASS " + _password, false);
                    readResponse(_visualConnectionParameters);
                    if (_response != USER_LOGGED_IN)
                        fail();
                    break;
                case USER_LOGGED_IN:
                    break;
                default:
                    fail();
                    break;
            }

            Syst();

#if DEMO
            doMessageBox("THIS IS A DEMO VERSION");
            doMessageBox(stringa);
            doMessageBox("DEVICE IS CONNECTED");
#endif

            return;
        }

        // Signals when the resolve has finished.
        private static ManualResetEvent GetHostEntryFinished =
            new ManualResetEvent(false);

        // Define the state object for the callback. 
        // Use hostName to correlate calls with the proper result.
        private class ResolveState
        {
            string hostName;
            IPHostEntry resolvedIPs;

            public ResolveState(string host)
            {
                hostName = host;
            }

            public IPHostEntry IPs
            {
                get { return resolvedIPs; }
                set { resolvedIPs = value; }
            }
            public string host
            {
                get { return hostName; }
                set { hostName = value; }
            }
        }

        // Record the IPs in the state object for later use.
        private void getHostEntryCallback(IAsyncResult ar)
        {
            try
            {

                ResolveState ioContext = (ResolveState)ar.AsyncState;

                ioContext.IPs = Dns.EndGetHostEntry(ar);
                GetHostEntryFinished.Set();
            }
            catch
            {
            }
        }

        // Determine the Internet Protocol (IP) addresses for 
        // this host asynchronously.
        private IPEndPoint doGetHostEntryAsync()
        {
            GetHostEntryFinished.Reset();
            ResolveState ioContext = new ResolveState(this._ftpServer);

            Dns.BeginGetHostEntry(ioContext.host,
                new AsyncCallback(getHostEntryCallback), ioContext);

            // Wait here until the resolve completes (the callback 
            // calls .Set())
            GetHostEntryFinished.WaitOne(_timeout,false);


            string s = string.Format("EndGetHostEntry({0}) returns:", ioContext.host);
			_logger.WriteLogFile(s);            

            if (ioContext.IPs == null)
                ioContext.IPs = Dns.GetHostEntry(ioContext.host);

            if (ioContext.IPs.AddressList.Length <= 0)
                fail("Error on Resolve server");

            // Verifico che tipo di rete mi viene ritornata
            int idx = 0;
            foreach (IPAddress ip in ioContext.IPs.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    break;
                idx++;
            }
            this._ftpServer = ioContext.IPs.AddressList[idx].ToString();
            return new IPEndPoint(ioContext.IPs.AddressList[idx], _port);

        }


#if DEMO
        private void doMessageBox(string stringa)
        {
            System.Windows.Forms.Classi.ClassInterfaccia.MessageBox(stringa, "DEMO DEMO DEMO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }
#endif

        /// <summary>
        /// Verify in a file exists
        /// </summary>
        /// <param name="fileName">Filename to verify</param>
        /// <returns>true = exist, false = not</returns>
        public bool ExistFile(string fileName)
        {

            string s = "SONO IN: existFile";
            _logger.WriteLogFile(s);

            if (checkConnection() == false)
                return false;

            string file_list = "";
            long bytesgot = 0;
            bool isSizeCommand = false;

            openDataSocket();

            /*
             * Il problema si ha su Unix in quanto non ha il comando SIZE.
             * Poi su Windows, il comando NLST se non c'è il file non restituisce niente
             * Quindi ho dovuto fare questo escamotage, nella speranza che non ci siano altre casistiche...
             */
            if (_syst == EnumSyst.Unix)
                sendCommand("NLST " + fileName);
            else
            {
                if(fileName != "" &&
                    fileName.IndexOf("*") < 0 &&
                    fileName.IndexOf("?") < 0)
                {
                    isSizeCommand = true;
                    sendCommand("SIZE " + fileName);
                }
                else
                    sendCommand("NLST " + fileName); // Purtroppo questa riga di codice rischia di bloccare l'ftp qualora non ci siano files... 
            }

            readResponse();

            int prevResponse = _response;

            switch (_response)
            {
                case DATA_CONNECTION_ALREADY_OPEN:
                case FILE_STATUS_OK:
                case TRANSFER_OK:
                    break;
                case FILE_STATUS:
                    if (isSizeCommand)
                    {
                        closeDataSocket();
                        return true;
                    }
                    fail();
                    break;
                case NO_SUCH_FILE:
                case DIRECTORY_NOT_FOUND:
                    closeDataSocket();
                    return false;
                default:
                    fail();
                    break;
            }

            
            /*
             *  Rimango in attesa che arrivino i dati o qualche errore
             * Per ovviare al problema
             */
            int newTimeout = (_syst == EnumSyst.Unix ? _timeout : 6000);
            if (isTimeout(_dataSock, newTimeout))
            {
                if(_syst == EnumSyst.Unix)
                    fail("Timeout from server");
            }

            while (_dataSock.Available > 0)
            {
                bytesgot = _dataSock.Receive(_bytesArray, _bytesArray.Length, 0);
                file_list += Encoding.ASCII.GetString(_bytesArray, 0, (int)bytesgot);
                System.Threading.Thread.Sleep(_sleepTime);
            }

            closeDataSocket();

            if (_syst == EnumSyst.Unix)
            {
                // In unix il codice 226 transfer complete viene inviato dopo la lista dei files, 
                // In windows prima ... mah
                // ATTENZIONE, quello che c'è scritto sopra non è sempre vero, quindi faccio il test sotto. Se ho già ricevuto il 226
                // non attendo altro...
                if (prevResponse != CLOSING_DATA_CONNECTION)
                {
                    readResponse();
                    if (_response != TRANSFER_OK)
                        fail();
                }
#if DEMO
                doMessageBox("'NSLT' COMMAND DONE");
#endif
            }

            // In alcuni server torna comunque TRANSFER_OK. E' per questo che controllo se in file_list c'è qualche dato
            // In altri server torna dei dati con "no such file or directory", quindi faccio una verifica anche su questo

            if (file_list == "")
                return false;
            else if (file_list.ToUpper().IndexOf("NO SUCH FILE OR DIRECTORY")>= 0)
                return false;
            else
                return true;

        }

        /// <summary>
        /// Do the directory LIST, Accept wildcards
        /// </summary>
        /// <param name="fileName">Filename to list</param>
        /// <returns>List files</returns>
        public List<FtpList> List(string fileName)
        {
            string s = "SONO IN: list";
            _logger.WriteLogFile(s);

            List<FtpList> lista = new List<FtpList>(0);

            if (checkConnection() == false)
                return lista;

            // Prima verifico se ci sono i files
            if (ExistFile(fileName) == false)
                return lista;

            string file_list = "";
            long bytesgot = 0;
            ArrayList list = new ArrayList();

            openDataSocket();
            sendCommand("LIST " + fileName);

            readResponse();

            //FILIPE MADUREIRA.
            //Added response 125
            int prevResponse = _response;
            switch (_response)
            {
                case DATA_CONNECTION_ALREADY_OPEN:
                case FILE_STATUS_OK:
                case TRANSFER_OK:
                    break;
                default:
                    fail();
                    break;
            }

            // Rimango in attesa che arrivino i dati o qualche errore
            if (isTimeout(_dataSock))
                fail("Timeout from server");

            while (_dataSock.Available > 0)
            {
                bytesgot = _dataSock.Receive(_bytesArray, _bytesArray.Length, 0);
                file_list += Encoding.ASCII.GetString(_bytesArray, 0, (int)bytesgot);
                System.Threading.Thread.Sleep(_sleepTime);
            }

            closeDataSocket();

            if (_syst == EnumSyst.Unix)
            {
                if (prevResponse != CLOSING_DATA_CONNECTION)
                {
                    readResponse();
                    if (_response != CLOSING_DATA_CONNECTION &&
                        _response != NO_SUCH_FILE)
                        fail();
                }
            }

            s = file_list;
            _logger.WriteLogFile(s);
            OnTextReceived(s);

            foreach (string f in file_list.Split('\n'))
            {
                if (f.Length > 0 && !Regex.Match(f, "^total").Success)
                {
                    FtpList ftpList = new FtpList();
                    ftpList.Fill(f.Substring(0, f.Length - 1), _syst);
                    lista.Add(ftpList);
                }
            }


#if DEMO
            doMessageBox("'LIST' COMMAND DONE");
#endif

            return lista;
        }


		/// <summary>
		/// Do CD command
		/// </summary>
		/// <param name="directory">Directory name</param>
		public void Cd(string directory)
		{

            string s = "SONO IN: Cd";
            _logger.WriteLogFile(s);


			try
			{

                if (checkConnection() == false)
                    return;

                //changeDir(Regex.Replace(directory, "cd ", ""));
                string path = Regex.Replace(directory, "cd ", "");
			    sendCommand("CWD " + path);
			    readResponse();
			    if (_response != REQUESTED_FILE_ACTION_OK)
				    throw new Exception(_responseStr);

#if DEMO
                doMessageBox("'CD' COMMAND DONE");
#endif			
            }
			catch(Exception ex)
			{
				fail(ex.Message);
			}
		}

//        private void removeFile(string filename)
//        {

//            writeLogFile("SONO IN: removeFile", false);

////			connect();
//            try
//            {
//                sendCommand("DELE " + filename);
//                readResponse();
//                if (response != REQUESTED_FILE_ACTION_OK)
//                    throw new Exception(responseStr);
//#if DEMO
//                doMessageBox("'DELE' COMMAND DONE");
//#endif
//            }
//            catch (Exception ex)
//            {
//                fail(ex.Message);
//            }
//        }

		private long getFileSize(string filename)
		{

            string s = "SONO IN: getFileSize";
            _logger.WriteLogFile(s);


			sendCommand("SIZE " + filename);
			readResponse();

            // Ci sono alcuni sistemi che non hanno il size oppure
            // se sono in ascii non viene accettato il size...
			if (_response == COMMAND_UNRECOGNIZED ||
                _response == SIZE_NOT_ALLOWED_IN_ASCII_MODE)
			{

				// Faccio una dir
				List<FtpList> list = List(filename);
                if (list.Count == 1)
                {
                    // Cerco di recuperare la size del file
                    if (_syst == EnumSyst.Unix)
                        return list[0].Size;
                    else
                        fail("SO WINDOWS NOT USED");
                }
                else
                    fail("FILE NOT FOUND " + filename);
			}
			else if (_response != FILE_STATUS)
				throw new Exception(_responseStr);

            return Int64.Parse(_responseStr.Substring(4));
		}

		private void openUpload(string localFilename, string remoteFilename, bool resume, bool append)
		{

            string s = "SONO IN: openUpload";
            _logger.WriteLogFile(s);


//			connect();
            //setBinaryMode(true);
			openDataSocket();

			_bytesTotal = 0;

			try
			{
				_file = new FileStream(localFilename, FileMode.Open);
			}
			catch(Exception ex)
			{
				_file = null;
				throw ex;
			}

			_fileSize = _file.Length;

			if(resume)
			{
				long size = getFileSize(remoteFilename);
				sendCommand("REST " + size);
				readResponse();
				if (_response == REQUESTED_FILE_ACTION_PENDING)
					_file.Seek(size, SeekOrigin.Begin);
			}
			
			if(append)
				sendCommand("APPE " + remoteFilename);
			else
				sendCommand("STOR " + remoteFilename);
			readResponse();

			switch(_response)
			{
				case DATA_CONNECTION_ALREADY_OPEN:
				case FILE_STATUS_OK:
					break;
				default:
					_file.Close();
					_file = null;
					throw new Exception(_responseStr);
			}

			return;
		}

		private void openDownload(string remoteFilename, string localFilename, bool resume)
		{

            string s = "SONO IN: openDownload " + remoteFilename + " " + localFilename;
            _logger.WriteLogFile(s);


			_bytesTotal = 0;
			
            _fileSize = getFileSize(remoteFilename);
 			
			if(resume && File.Exists(remoteFilename))
			{
				try
				{
					_file = new FileStream(remoteFilename, FileMode.Open);
				}
				catch(Exception ex)
				{
					_file = null;
					throw ex;
				}

				sendCommand("REST " + _file.Length);
				readResponse();
				if (_response != REQUESTED_FILE_ACTION_PENDING)
					throw new Exception(_responseStr);
				_file.Seek(_file.Length, SeekOrigin.Begin);
				_bytesTotal = _file.Length;
			}
			else
			{
				try
				{
					_file = new FileStream(localFilename, FileMode.Create);
				}
				catch(Exception ex)
				{
					_file = null;
                    fail(ex.Message + " sul file " + remoteFilename);					
				}
			}

			openDataSocket();
			sendCommand("RETR " + remoteFilename);
		
			readResponse();
			
			switch(_response)
			{
				case DATA_CONNECTION_ALREADY_OPEN:
				case FILE_STATUS_OK:
                case CLOSING_DATA_CONNECTION: // Per alcuni unix tipo filezilla
					break;
				default:
					_file.Close();
					_file = null;
					throw new Exception(_responseStr);
			}

			return;
		}

		private int doUpload()
		{

            string s = "SONO IN: doUpload";
            _logger.WriteLogFile(s);


			//Byte[] bytes = new Byte[byteArraySize];
			long bytes_got;

			bytes_got = _file.Read(_bytesArray, 0, _bytesArray.Length);
			_bytesTotal += bytes_got;
			_dataSock.Send(_bytesArray, (int)bytes_got, 0);

			if(_bytesTotal == _fileSize)
			{
                //_file.Close();
                //_file = null;
				
                closeDataSocket();

				readResponse();
				
				switch(_response)
				{
					case CLOSING_DATA_CONNECTION:
					case REQUESTED_FILE_ACTION_OK:
						break;
					default:
						throw new Exception(_responseStr);
				}
				
			}

			//FILIPE MADUREIRA
			//To prevent DIVIDEBYZERO Exception on zero length files
			if(_fileSize == 0)
			{
				// consider throwing an exception here so the calling code knows
				// there was an error
				return 100;
			}

			return (int)((_bytesTotal * 100) / _fileSize);
		}

        private bool isTimeout(Socket socket, int newTimeout)
        {
            int seconds_passed = 0;


            // Ho inserito il mainsock.available in quanto
            // devo verificare quali dei due socket ottiene per primo dei dati...
            while (socket.Available <= 0 && _mainSock.Available <= 0)
            {
                System.Threading.Thread.Sleep(_sleepTime);
                seconds_passed += _sleepTime;

                if (seconds_passed > newTimeout)
                    return true;

            }
            return false;
        }

        private bool isTimeout(Socket socket)
        {
            return isTimeout(socket, _timeout);
        }

		private int doDownload()
		{

			//writeLogFile("SONO IN: doDownload", false);

			//Byte[] bytes = new Byte[byteArraySize];
			long bytes_got;

            // Questa verifica è per i files a lunghezza "0"
            if (_fileSize != 0) 
            {
                if (isTimeout(_dataSock))
                    fail("Timeout from server");

                bytes_got = _dataSock.Receive(_bytesArray, _bytesArray.Length, 0);
                _file.Write(_bytesArray, 0, (int)bytes_got);

                _bytesTotal += bytes_got;
            }

			if(_bytesTotal >= _fileSize && _dataSock.Available == 0) // >= perchè quando ho dei file su unix e trasferisco in ascii, mi tornano + bytes per il problema del CRLF
			{
                // Modifica effettuata con la versione 2.13.3                
                //_file.Close();
                //_file = null;

                // Questo serve per avere una percentuale corretta, altrimenti per i files trasferiti da unix
                // in formato ASCII ho dei files + lunghi su windows
                if (_bytesTotal > _fileSize)
                    _fileSize = _bytesTotal;

                closeDataSocket();
                
                if (_response != CLOSING_DATA_CONNECTION)
                {

                    readResponse();

                    switch (_response)
                    {
                        case CLOSING_DATA_CONNECTION:
                        case REQUESTED_FILE_ACTION_OK:
                            break;
                        default:
                            throw new Exception(_responseStr);
                        //break;
                    }
                }
				
				//FILIPE MADUREIRA
				//To prevent DIVIDEBYZERO Exception on zero length files
				if(_fileSize == 0)
				{
					// consider throwing and exception here so the calling code knows
					// there was an error
					return 100;
				}
			}

			return (int)((_bytesTotal * 100) / _fileSize);
		}

        /// <summary>
        /// FTP Server
        /// </summary>
        public string FtpServer
        {
            get
            {
                return this._ftpServer;
            }
            set
            {
                this._ftpServer = value;
            }
        }
        
        /// <summary>
        /// FTP Username
        /// </summary>
		public string UserName
		{
			get
			{
				return this._userName;
			}
            set
            {
                _userName = value;
            }
		}

        /// <summary>
        /// Ftp password
        /// </summary>
		public string Password
		{
			get
			{
                return this._password;
			}
            set
            {
                this._password = value;
            }

		}

        public string RasEntry
        {
            get { return this._rasEntry; }
            set { this._rasEntry = value; }
        }

        public string RasUserName
        {
            get { return this._rasUserName; }
            set { this._rasUserName = value; }
        }

        public string RasPassword
        {
            get { return this._rasPassword; }
            set { this._rasPassword = value; }
        }

        /// <summary>
        /// FTP Port
        /// </summary>
		public int Port
		{
			get
			{
				return this._port;
			}
            set
            {
                _port = value;
            }
		}

        public bool VisualConnectionParameters
        {
            get { return _visualConnectionParameters; }
            set { _visualConnectionParameters = value; }
        }

		/// <summary>
		/// Timeout in milliseconds
		/// </summary>
		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				this._timeout = value;
			}
		}

        /// <summary>
        /// SleepTime for long time commands, in millisecond
        /// </summary>
        public int SleepTime
        {
            get
            {
                return this._sleepTime;
            }
            set
            {
                _sleepTime = value;
            }
        }

        /// <summary>
        /// String1 used in configFile (as you like...)
        /// </summary>
        public string String1
        {
            get
            {
                return _string1;
            }
            set
            {
                _string1 = value;
            }
        }

        /// <summary>
        /// String2 used in configFile (as you like...)
        /// </summary>
        public string String2
        {
            get
            {
                return _string2;
            }
            set
            {
                _string2 = value;
            }
        }

        public static bool IsPort(string port)
        {
            bool ok = false;
            try
            {
                System.Convert.ToInt32(port);
                ok = true;
            }
            catch
            {

            }
            return ok;
        }
        
        protected virtual void OnTextReceived(string s)
        {
            if (FtpMessages != null)
            {
                s = s.Replace("\n", "");
                s = s.Replace("\r", ((char)13).ToString() + ((char)10).ToString());
                if (s.IndexOf('\r') < 0)
                    s += ((char)13).ToString() + ((char)10).ToString();
                FtpMessagesEventArgs e = new FtpMessagesEventArgs(s);
                FtpMessages(this, e);
            }
        }

        protected virtual void OnChangePercent(FtpTransferPercentageEventArgs e)
        {
            if (FtpTransferPercentage != null)
            {
                FtpTransferPercentage(this, e);
            }
        }

		private void upload(string localFilename, string remoteFilename, bool resume, bool append)
		{
            string s = String.Format("SONO IN: upload localFilename = {0}, remoteFilename = {1}, resume = {2}, append = {3}", localFilename, remoteFilename, resume, append);
            _logger.WriteLogFile(s);


			try
			{
				int perc = 0, oldPerc = 0;
				//				string file = Regex.Replace(command, "put ", "");

                if (checkConnection() == false)
                    return;

				// open an upload
				//				ftplib.OpenUpload(file, System.IO.Path.GetFileName(file));
				openUpload(localFilename, remoteFilename, resume, append);
				while (true)
				{
					// the DoUpload() function returns the percentage complete in the transfer
					// there are 2 public members bytes_total and file_size
					// that may be of interest to the developer. They are used by
					// the upload/download code though so DO NOT modify them. I know
					// it's sloppy to make them public rather than a 'get' function.
					perc = doUpload();
					if (perc > oldPerc)
					{
						string stringa = "Uploading: \r\nFrom: " + localFilename + "\r\n---\r\nTo: " + remoteFilename + "\r\n" + _bytesTotal.ToString() + "/" + _fileSize.ToString() + " " + perc.ToString() + "%";

                        FtpTransferPercentageEventArgs e = new FtpTransferPercentageEventArgs(localFilename, FtpTransferPercentageEventArgs.EnumTransferType.Upload, perc, _bytesTotal, _fileSize);
                        OnChangePercent(e);
                        OnTextReceived(stringa);
						//Console.Write("\rUpload: {0}/{1} {2}%", ftplib.bytes_total, ftplib.file_size, perc);
						//						Console.Out.Flush();
					}

					oldPerc = perc;

					if (perc == 100)
						break;
				}
#if DEMO
                doMessageBox("'UPLOAD' DONE");
#endif
            }
			catch (Exception ex)
			{
				fail(ex.Message);
			}
			finally
			{
				closeDataSocket();
			}
		}

        /// <summary>
        /// Verifica se c'è la connessione ed eventualmente genera un errore
        /// </summary>
        /// <returns>False = non connesso</returns>
        private bool checkConnection()
        {
            if (!IsConnected())
            {
                string s = "E: Must be connected to a server.";
                _logger.WriteLogFile(s);
                OnTextReceived(s);

                return false;
            }
            else
                return true;

        }

		/// <summary>
		/// Send file to server
		/// </summary>
		/// <param name="localFilename">Local filename</param>
		/// <param name="remoteFilename">Remote filename</param>
		/// <param name="resume">Resume previous connection (TO TEST)</param>
		public void Upload(string localFilename,string remoteFilename, bool resume)
		{
            string s = "SONO IN: Upload";
            _logger.WriteLogFile(s);

			upload(localFilename, remoteFilename, resume, false);
		}

        /// <summary>
        /// Append file to server
        /// </summary>
        /// <param name="localFilename">Local filename</param>
        /// <param name="remoteFilename">Remote filename</param>
		public void Append(string localFilename, string remoteFilename)
		{
            string s = "SONO IN: Append";
            _logger.WriteLogFile(s);

			upload(localFilename, remoteFilename, false, true);
		}

		/// <summary>
		/// Get file from server
		/// </summary>
		/// <param name="remoteFilename">Remote filename</param>
		/// <param name="localFilename">Local filename</param>
		/// <param name="resume">Resume previous connection (TO TEST)</param>
		public void Download(string remoteFilename, string localFilename,bool resume)
		{

            string s = "SONO IN: Download";
            _logger.WriteLogFile(s);

			try
			{
				int perc = 0, oldPerc = 0;


                if (checkConnection() == false)
                    return;

				// open the file with resume support if it already exists, the last
				// peram should be false for no resume
				//OpenDownload(Regex.Replace(command, "get ", ""), false);
				openDownload(remoteFilename, localFilename, resume);
				while (true)
				{
					// the DoDownload() function returns the percentage complete
					// there are 2 public members bytes_total and file_size
					// that may be of interest to the developer. They are used by
					// the upload/download code though so DO NOT modify them. I know
					// it's sloppy to make them public rather than a 'get' function.
					perc = doDownload();
					if (perc > oldPerc)
					{
						string stringa = "Downloading: \r\nFrom: " + remoteFilename + "\r\n---\r\nTo: " + localFilename + "\r\n" + _bytesTotal.ToString() + "/" + _fileSize.ToString() + " " + perc.ToString() + "%";
                        FtpTransferPercentageEventArgs e = new FtpTransferPercentageEventArgs(remoteFilename, FtpTransferPercentageEventArgs.EnumTransferType.Download, perc, _bytesTotal, _fileSize);
                        OnChangePercent(e);
                        //writeTesto(stringa, false);
                        OnTextReceived(stringa);
					}

					oldPerc = perc;

					if (perc >= 100)
						break;
                }
#if DEMO
                doMessageBox("'DOWNLOAD' DONE");
#endif
            }
			catch (Exception ex)
			{
				fail(ex.Message);
			}
			finally
			{
				closeDataSocket();
			}
		}

        /// <summary>
        /// Do PWD Command
        /// </summary>
        /// <returns>The actual remote directory</returns>
        public string Pwd()
        {
            string s = "SONO IN: Pwd";
            _logger.WriteLogFile(s);

            try
            {
                if (checkConnection() == false)
                    return "";

                //makeDir(Regex.Replace(command, "mkdir ", ""));
                //makeDir(dir);
                sendCommand("PWD");
                readResponse();

                switch (_response)
                {
                    case PATHNAME_CREATED:
                    case REQUESTED_FILE_ACTION_OK:
                        break;
                    default:
                        throw new Exception(_responseStr);
                }
#if DEMO
                doMessageBox("'MKD' COMMAND DONE");
#endif
            }
            catch (Exception ex)
            {
                fail(ex.Message);
                //writeLogFile(ex.Message, true);
                //throw ex;
            }
            return _responseStr;
        }

		/// <summary>
		/// Do MKDIR Command
		/// </summary>
		/// <param name="dir"></param>
		public void Mkdir(string dir)
		{

            string s = "SONO IN: Mkdir";
            _logger.WriteLogFile(s);

			try
			{
                if (checkConnection() == false)
                    return;

				//makeDir(Regex.Replace(command, "mkdir ", ""));
				//makeDir(dir);
    			sendCommand("MKD " + dir);
			    readResponse();
			
    			switch(_response)
    			{
	    			case PATHNAME_CREATED:
	    			case REQUESTED_FILE_ACTION_OK:
	    				break;
    				default:
    					throw new Exception(_responseStr);
    			}
#if DEMO
                 doMessageBox("'MKD' COMMAND DONE");
#endif
            }
			catch(Exception ex)
			{
                fail(ex.Message);
                //writeLogFile(ex.Message, true);
                //throw ex;
			}
		}	

		/// <summary>
		/// Do RMDIR Command
		/// </summary>
		/// <param name="dir"></param>
		public void Rmdir(string dir)
		{

            string s = "SONO IN: Rmdir";
            _logger.WriteLogFile(s);

			try
			{
                if (checkConnection() == false)
                    return;

				//removeDir(dir);
                sendCommand("RMD " + dir);
                readResponse();
                if (_response != REQUESTED_FILE_ACTION_OK)
                    throw new Exception(_responseStr);
#if DEMO
                doMessageBox("'RMD' COMMAND DONE");
#endif
            }
			catch(Exception ex)
			{
                fail(ex.Message);
                //writeLogFile(ex.Message, true);
                //throw ex;
			}
		}

        /// <summary>
        /// Do SYST Command
        /// </summary>
		public void Syst()
		{

            string s = "SONO IN: Syst";
            _logger.WriteLogFile(s);

			try
			{
                if (checkConnection() == false)
                    return;
                
                sendCommand("SYST");
				readResponse();
				if (_response != SYSTEM_TYPE)
					throw new Exception(_responseStr);
                if (_responseStr.ToUpper().IndexOf("UNIX") >= 0)
                    _syst = EnumSyst.Unix;
                else
                    _syst = EnumSyst.Windows;
			}
			catch (Exception ex)
			{
                fail(ex.Message);
                //writeLogFile(ex.Message, true);
                //throw ex;
			}
		}

        /// <summary>
        /// Do HELP Command
        /// </summary>
		public void Help()
		{

            string s = "SONO IN: Help";
            _logger.WriteLogFile(s);

			try
			{
                if (checkConnection() == false)
                    return;
                
                sendCommand("HELP");
				readResponse();
			}
			catch (Exception ex)
			{
                fail(ex.Message);
                //writeLogFile(ex.Message, true);
                //throw ex;
			}
		}


		/// <summary>
		/// Close the connection
		/// </summary>
		public void Close()
		{

            string s = "SONO IN: Close";
            _logger.WriteLogFile(s);

            //// se ho in precedenza salvato l'ftpserver, la riassegno
            //if (saveFtpServer != "")
            //    ftpServer = saveFtpServer;

			try
			{
                try
                {
                    closeDataSocket(); // Chiudo l'eventuale socket
                }
                catch
                {
                }

                // Chiudo il mainSocket
				if (IsConnected())
				{
                    s = "--> Disconnessione.";
                    _logger.WriteLogFile(s);
                    OnTextReceived(s);

					disconnect();
				}

			}
			catch (Exception ex)
			{
                //writeLogFile(ex.Message, true);
                //throw ex;
                s = ex.Message;
                _logger.WriteLogFile(s);
                OnTextReceived(s);

                //fail(ex.Message);
			}
			finally
			{
				this._bytesArray = null;
			}
		}

		/// <summary>
		/// Erase a file
		/// </summary>
		/// <param name="fileName">Filename to erase</param>
		public void Rm(string fileName)
		{

            string s = "SONO IN: Rm";
            _logger.WriteLogFile(s);


			try
			{
                if (checkConnection() == false)
                    return;

                sendCommand("DELE " + fileName);
                readResponse();
                if (_response != REQUESTED_FILE_ACTION_OK)
                    throw new Exception(_responseStr);
#if DEMO
                doMessageBox("'DELE' COMMAND DONE");
#endif
            }
			catch(Exception ex)
			{
                //writeLogFile(ex.Message, true);
                //throw ex;
                fail(ex.Message);
			}
		}


		/// <summary>
		/// Rename a file
		/// </summary>
		/// <param name="fileName">Remote filename to rename</param>
		/// <param name="newFileName">New remote filename</param>
		public void RenameFile(string fileName, string newFileName)
		{

            string s = "SONO IN: RenameFile";
            _logger.WriteLogFile(s);

			try
			{
                if (checkConnection() == false)
                    return;

                sendCommand("RNFR " + fileName);
                readResponse();
                if (_response != REQUESTED_FILE_ACTION_PENDING)
                    throw new Exception(_responseStr);

                sendCommand("RNTO " + newFileName);
                readResponse();
                if (_response != REQUESTED_FILE_ACTION_OK)
                    throw new Exception(_responseStr);
            }
			catch (Exception ex)
			{
                //writeLogFile(ex.Message, true);
                //throw ex;
                fail(ex.Message);
			}
		}

//        private void renameFile(string filename, string newFileName)
//        {

//            writeLogFile("SONO IN: renameFile", false);

////			connect();
//            sendCommand("RNFR " + filename );
//            readResponse();
//            if (response != REQUESTED_FILE_ACTION_PENDING)
//                throw new Exception(responseStr);

//            sendCommand("RNTO " + newFileName);
//            readResponse();
//            if (response != REQUESTED_FILE_ACTION_OK)
//                throw new Exception(responseStr);

//        }

        /// <summary>
        /// Name of config file
        /// </summary>
		public string ConfigFile
		{
			get
			{
				return this._configFile;
			}
			set
			{
				this._configFile = value;
			}
		}

        public long FileSize
        {
            get { return this._fileSize; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public bool ViewAllInitData
        {
            get { return _viewAllInitData; }
            set { _viewAllInitData = value; }
        }
	}

    class SslHelper : IDisposable
    {

        private const ushort SO_SECURE = 0x2001;

        private const ushort SO_SEC_SSL = 0x2004;

        private const int _SO_SSL_FLAGS = 0x02;

        private const int _SO_SSL_VALIDATE_CERT_HOOK = 0x08;

        private const int SO_SSL_FAMILY = 0x00730000;

        private const long _SO_SSL = ((2L << 27) | SO_SSL_FAMILY);

        private const uint IOC_IN = 0x80000000;

        private const long SO_SSL_SET_VALIDATE_CERT_HOOK = (IOC_IN | _SO_SSL | _SO_SSL_VALIDATE_CERT_HOOK);

        private const long SO_SSL_SET_FLAGS = (IOC_IN | _SO_SSL | _SO_SSL_FLAGS);

        private const int SSL_CERT_X59 = 1;

        private const int SSL_ERR_OKAY = 0;

        private const int SSL_ERR_FAILED = 2;

        private const int SSL_ERR_BAD_LEN = 3;

        private const int SSL_ERR_BAD_TYPE = 4;

        private const int SSL_ERR_BAD_DATA = 5;

        private const int SSL_ERR_NO_CERT = 6;

        private const int SSL_ERR_BAD_SIG = 7;

        private const int SSL_ERR_CERT_EXPIRED = 8;

        private const int SSL_ERR_CERT_REVOKED = 9;

        private const int SSL_ERR_CERT_UNKNOWN = 10;

        private const int SSL_ERR_SIGNATURE = 11;

        private const int SSL_CERT_FLAG_ISSUER_UNKNOWN = 0x0001;

        public delegate int SSLVALIDATECERTFUNC(uint dwType, IntPtr pvArg, uint dwChainLen, IntPtr pCertChain, uint dwFlags);

        private IntPtr ptrHost;

        private IntPtr hookFunc;

        public SslHelper(Socket socket, string host, bool deferHandshake)
        {

            //The managed SocketOptionName enum doesn't have SO_SECURE so here we cast the integer value

            socket.SetSocketOption(SocketOptionLevel.Socket, (SocketOptionName)SO_SECURE, SO_SEC_SSL);



            //We need to pass a function pointer and a pointer to a string containing the host

            //to unmanaged code

            hookFunc = Marshal.GetFunctionPointerForDelegate(new SSLVALIDATECERTFUNC(ValidateCert));



            //Allocate the buffer for the string

            ptrHost = Marshal.AllocHGlobal(host.Length + 1);

            WriteASCIIString(ptrHost, host);



            //Now put both pointers into a byte[]

            var inBuffer = new byte[8];

            var hookFuncBytes = BitConverter.GetBytes(hookFunc.ToInt32());

            var hostPtrBytes = BitConverter.GetBytes(ptrHost.ToInt32());

            Array.Copy(hookFuncBytes, inBuffer, hookFuncBytes.Length);

            Array.Copy(hostPtrBytes, 0, inBuffer, hookFuncBytes.Length, hostPtrBytes.Length);

            unchecked
            {

                socket.IOControl((int)SO_SSL_SET_VALIDATE_CERT_HOOK, inBuffer, null);
                if (deferHandshake)
                    socket.IOControl((int)SO_SSL_SET_FLAGS, BitConverter.GetBytes(SSL_FLAG_DEFER_HANDSHAKE), null);
            }
        }



        private static void WriteASCIIString(IntPtr basePtr, string s)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(s);

            for (int i = 0; i < bytes.Length; i++)

                Marshal.WriteByte(basePtr, i, bytes[i]);



            //null terminate the string

            Marshal.WriteByte(basePtr, bytes.Length, 0);

        }



        #region IDisposable Members



        ~SslHelper()
        {

            ReleaseHostPointer();

        }



        public void Dispose()
        {

            GC.SuppressFinalize(this);

            ReleaseHostPointer();

        }



        private void ReleaseHostPointer()
        {

            if (ptrHost != IntPtr.Zero)
            {

                Marshal.FreeHGlobal(ptrHost);

                ptrHost = IntPtr.Zero;

            }

        }



        #endregion



        private int ValidateCert(uint dwType, IntPtr pvArg, uint dwChainLen, IntPtr pCertChain, uint dwFlags)
        {

            //According to http://msdn.microsoft.com/en-us/library/ms940451.aspx:

            //

            //- dwChainLen is always 1

            //- Windows CE performs the cert chain validation

            //- pvArg is the context data we passed into the SO_SSL_SET_VALIDATE_CERT_HOOK call so in our

            //- case is the host name

            //

            //So here we are responsible for validating the dates on the certificate and the CN





            if (dwType != SSL_CERT_X59)

                return SSL_ERR_BAD_TYPE;



            //When in debug mode let self-signed certificates through ...

#if !DEBUG

            if ((dwFlags & SSL_CERT_FLAG_ISSUER_UNKNOWN) != 0)

                return SSL_ERR_CERT_UNKNOWN;

#endif



            Debug.Assert(dwChainLen == 1);



            //Note about the note: an unmanaged long is 32 bits, unlike a managed long which is 64. I was missing

            //this fact when I wrote the comment. So the docs are accurate.

            //NOTE: The documentation says pCertChain is a pointer to a LPBLOB struct:

            //

            // {ulong size, byte* data}

            //

            //in reality the size is a 32 bit integer (not 64).

            int certSize = Marshal.ReadInt32(pCertChain);

            IntPtr pData = Marshal.ReadIntPtr(new IntPtr(pCertChain.ToInt32() + sizeof(int)));



            byte[] certData = new byte[certSize];



            for (int i = 0; i < certSize; i++)

                certData[i] = Marshal.ReadByte(pData, (int)i);



            System.Security.Cryptography.X509Certificates.X509Certificate cert;

            try
            {

                cert = new X509Certificate(certData);

            }

            catch (ArgumentException) { return SSL_ERR_BAD_DATA; }

            catch (CryptographicException) { return SSL_ERR_BAD_DATA; }



            //Validate the expiration date

            if (DateTime.Now > DateTime.Parse(cert.GetExpirationDateString(), CultureInfo.CurrentCulture))

                return SSL_ERR_CERT_EXPIRED;



            //Validate the effective date

            if (DateTime.Now < DateTime.Parse(cert.GetEffectiveDateString(), CultureInfo.CurrentCulture))

                return SSL_ERR_FAILED;



            string certName = cert.GetName();

            Debug.WriteLine(certName);



            //Validate the CN

            string host = ReadAnsiString(pvArg);

            if (certName.IndexOf("CN=" + host)<0)

                return SSL_ERR_FAILED;



            return SSL_ERR_OKAY;

        }



        private static string ReadAnsiString(IntPtr pvArg)
        {

            byte[] buffer = new byte[1024];

            int j = 0;

            do
            {

                buffer[j] = Marshal.ReadByte(pvArg, j);

                j++;

            } while (buffer[j - 1] != 0);

            string host = Encoding.ASCII.GetString(buffer, 0, j - 1);

            return host;

        }

        private const int SSL_FLAG_DEFER_HANDSHAKE = 0x0008;
        private const int _SO_SSL_PERFORM_HANDSHAKE = 0x0d;
        private const long SO_SSL_PERFORM_HANDSHAKE = _SO_SSL | _SO_SSL_PERFORM_HANDSHAKE;

        //public void DoHandshake()
        //{

        //    socket.IOControl((int)SO_SSL_PERFORM_HANDSHAKE, BitConverter.GetBytes(ptrHost.ToInt32()), null);

        //}
    }
}