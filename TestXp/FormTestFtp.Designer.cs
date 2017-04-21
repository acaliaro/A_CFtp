namespace TestXp
{
    partial class FormTestFtp
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSendFile = new System.Windows.Forms.Button();
            this.textBoxLocalFileName = new System.Windows.Forms.TextBox();
            this.textBoxRemoteFileName = new System.Windows.Forms.TextBox();
            this.buttonReceiveFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxRemoteDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonDir = new System.Windows.Forms.Button();
            this.buttonAppendFile = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSendCommand = new System.Windows.Forms.TextBox();
            this.buttonSendCmd = new System.Windows.Forms.Button();
            this.textBoxEvent = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxCd = new System.Windows.Forms.TextBox();
            this.buttonCd = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxMkDir = new System.Windows.Forms.TextBox();
            this.buttonMkdir = new System.Windows.Forms.Button();
            this.buttonWriteConfigFile = new System.Windows.Forms.Button();
            this.buttonReadConfigFile = new System.Windows.Forms.Button();
            this.buttonPwd = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(84, 1);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(188, 20);
            this.textBoxServer.TabIndex = 0;
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(84, 27);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(188, 20);
            this.textBoxUser.TabIndex = 1;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(84, 53);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(188, 20);
            this.textBoxPassword.TabIndex = 2;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(280, 56);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.Location = new System.Drawing.Point(4, 144);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(460, 104);
            this.textBoxLog.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Ftp server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "User";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Pwd";
            // 
            // buttonSendFile
            // 
            this.buttonSendFile.Location = new System.Drawing.Point(280, 88);
            this.buttonSendFile.Name = "buttonSendFile";
            this.buttonSendFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSendFile.TabIndex = 8;
            this.buttonSendFile.Text = "Send file";
            this.buttonSendFile.UseVisualStyleBackColor = true;
            this.buttonSendFile.Click += new System.EventHandler(this.click);
            // 
            // textBoxLocalFileName
            // 
            this.textBoxLocalFileName.Location = new System.Drawing.Point(84, 84);
            this.textBoxLocalFileName.Name = "textBoxLocalFileName";
            this.textBoxLocalFileName.Size = new System.Drawing.Size(188, 20);
            this.textBoxLocalFileName.TabIndex = 9;
            // 
            // textBoxRemoteFileName
            // 
            this.textBoxRemoteFileName.Location = new System.Drawing.Point(84, 112);
            this.textBoxRemoteFileName.Name = "textBoxRemoteFileName";
            this.textBoxRemoteFileName.Size = new System.Drawing.Size(188, 20);
            this.textBoxRemoteFileName.TabIndex = 10;
            // 
            // buttonReceiveFile
            // 
            this.buttonReceiveFile.Location = new System.Drawing.Point(280, 112);
            this.buttonReceiveFile.Name = "buttonReceiveFile";
            this.buttonReceiveFile.Size = new System.Drawing.Size(75, 23);
            this.buttonReceiveFile.TabIndex = 11;
            this.buttonReceiveFile.Text = "Receive file";
            this.buttonReceiveFile.UseVisualStyleBackColor = true;
            this.buttonReceiveFile.Click += new System.EventHandler(this.click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "local filename";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "remote filename";
            // 
            // textBoxRemoteDir
            // 
            this.textBoxRemoteDir.Location = new System.Drawing.Point(80, 272);
            this.textBoxRemoteDir.Name = "textBoxRemoteDir";
            this.textBoxRemoteDir.Size = new System.Drawing.Size(176, 20);
            this.textBoxRemoteDir.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "REMOTE DIR";
            // 
            // buttonDir
            // 
            this.buttonDir.Location = new System.Drawing.Point(304, 272);
            this.buttonDir.Name = "buttonDir";
            this.buttonDir.Size = new System.Drawing.Size(75, 23);
            this.buttonDir.TabIndex = 16;
            this.buttonDir.Text = "Do DIR";
            this.buttonDir.UseVisualStyleBackColor = true;
            this.buttonDir.Click += new System.EventHandler(this.click);
            // 
            // buttonAppendFile
            // 
            this.buttonAppendFile.Location = new System.Drawing.Point(392, 84);
            this.buttonAppendFile.Name = "buttonAppendFile";
            this.buttonAppendFile.Size = new System.Drawing.Size(75, 23);
            this.buttonAppendFile.TabIndex = 17;
            this.buttonAppendFile.Text = "Append file";
            this.buttonAppendFile.UseVisualStyleBackColor = true;
            this.buttonAppendFile.Click += new System.EventHandler(this.click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(392, 120);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 18;
            this.buttonHelp.Text = "Do Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.click);
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(352, 8);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 19;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 304);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "TEST CMD:";
            // 
            // textBoxSendCommand
            // 
            this.textBoxSendCommand.Location = new System.Drawing.Point(80, 296);
            this.textBoxSendCommand.Name = "textBoxSendCommand";
            this.textBoxSendCommand.Size = new System.Drawing.Size(176, 20);
            this.textBoxSendCommand.TabIndex = 21;
            // 
            // buttonSendCmd
            // 
            this.buttonSendCmd.Location = new System.Drawing.Point(304, 296);
            this.buttonSendCmd.Name = "buttonSendCmd";
            this.buttonSendCmd.Size = new System.Drawing.Size(75, 23);
            this.buttonSendCmd.TabIndex = 22;
            this.buttonSendCmd.Text = "Send cmd";
            this.buttonSendCmd.UseVisualStyleBackColor = true;
            this.buttonSendCmd.Click += new System.EventHandler(this.click);
            // 
            // textBoxEvent
            // 
            this.textBoxEvent.Location = new System.Drawing.Point(0, 392);
            this.textBoxEvent.Multiline = true;
            this.textBoxEvent.Name = "textBoxEvent";
            this.textBoxEvent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEvent.Size = new System.Drawing.Size(480, 96);
            this.textBoxEvent.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 328);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "CD:";
            // 
            // textBoxCd
            // 
            this.textBoxCd.Location = new System.Drawing.Point(80, 320);
            this.textBoxCd.Name = "textBoxCd";
            this.textBoxCd.Size = new System.Drawing.Size(176, 20);
            this.textBoxCd.TabIndex = 25;
            // 
            // buttonCd
            // 
            this.buttonCd.Location = new System.Drawing.Point(304, 320);
            this.buttonCd.Name = "buttonCd";
            this.buttonCd.Size = new System.Drawing.Size(75, 23);
            this.buttonCd.TabIndex = 26;
            this.buttonCd.Text = "Do CD";
            this.buttonCd.UseVisualStyleBackColor = true;
            this.buttonCd.Click += new System.EventHandler(this.click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 352);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "MKDIR:";
            // 
            // textBoxMkDir
            // 
            this.textBoxMkDir.Location = new System.Drawing.Point(80, 344);
            this.textBoxMkDir.Name = "textBoxMkDir";
            this.textBoxMkDir.Size = new System.Drawing.Size(176, 20);
            this.textBoxMkDir.TabIndex = 28;
            // 
            // buttonMkdir
            // 
            this.buttonMkdir.Location = new System.Drawing.Point(304, 344);
            this.buttonMkdir.Name = "buttonMkdir";
            this.buttonMkdir.Size = new System.Drawing.Size(75, 23);
            this.buttonMkdir.TabIndex = 29;
            this.buttonMkdir.Text = "Do MKDIR";
            this.buttonMkdir.UseVisualStyleBackColor = true;
            this.buttonMkdir.Click += new System.EventHandler(this.click);
            // 
            // buttonWriteConfigFile
            // 
            this.buttonWriteConfigFile.Location = new System.Drawing.Point(392, 32);
            this.buttonWriteConfigFile.Name = "buttonWriteConfigFile";
            this.buttonWriteConfigFile.Size = new System.Drawing.Size(75, 23);
            this.buttonWriteConfigFile.TabIndex = 30;
            this.buttonWriteConfigFile.Text = "Write config file";
            this.buttonWriteConfigFile.UseVisualStyleBackColor = true;
            this.buttonWriteConfigFile.Click += new System.EventHandler(this.click);
            // 
            // buttonReadConfigFile
            // 
            this.buttonReadConfigFile.Location = new System.Drawing.Point(392, 56);
            this.buttonReadConfigFile.Name = "buttonReadConfigFile";
            this.buttonReadConfigFile.Size = new System.Drawing.Size(75, 23);
            this.buttonReadConfigFile.TabIndex = 31;
            this.buttonReadConfigFile.Text = "Read config file";
            this.buttonReadConfigFile.UseVisualStyleBackColor = true;
            this.buttonReadConfigFile.Click += new System.EventHandler(this.click);
            // 
            // buttonPwd
            // 
            this.buttonPwd.Location = new System.Drawing.Point(384, 272);
            this.buttonPwd.Name = "buttonPwd";
            this.buttonPwd.Size = new System.Drawing.Size(75, 23);
            this.buttonPwd.TabIndex = 32;
            this.buttonPwd.Text = "Do PWD";
            this.buttonPwd.UseVisualStyleBackColor = true;
            this.buttonPwd.Click += new System.EventHandler(this.click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(32, 256);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(416, 8);
            this.progressBar2.TabIndex = 34;
            // 
            // FormTestFtp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 481);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.buttonPwd);
            this.Controls.Add(this.buttonReadConfigFile);
            this.Controls.Add(this.buttonWriteConfigFile);
            this.Controls.Add(this.buttonMkdir);
            this.Controls.Add(this.textBoxMkDir);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonCd);
            this.Controls.Add(this.textBoxCd);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxEvent);
            this.Controls.Add(this.buttonSendCmd);
            this.Controls.Add(this.textBoxSendCommand);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonAppendFile);
            this.Controls.Add(this.buttonDir);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxRemoteDir);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonReceiveFile);
            this.Controls.Add(this.textBoxRemoteFileName);
            this.Controls.Add(this.textBoxLocalFileName);
            this.Controls.Add(this.buttonSendFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.textBoxServer);
            this.Name = "FormTestFtp";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxLog;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonSendFile;
		private System.Windows.Forms.TextBox textBoxLocalFileName;
		private System.Windows.Forms.TextBox textBoxRemoteFileName;
		private System.Windows.Forms.Button buttonReceiveFile;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxRemoteDir;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button buttonDir;
		private System.Windows.Forms.Button buttonAppendFile;
		private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxSendCommand;
        private System.Windows.Forms.Button buttonSendCmd;
        private System.Windows.Forms.TextBox textBoxEvent;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxCd;
        private System.Windows.Forms.Button buttonCd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxMkDir;
        private System.Windows.Forms.Button buttonMkdir;
        private System.Windows.Forms.Button buttonWriteConfigFile;
        private System.Windows.Forms.Button buttonReadConfigFile;
        private System.Windows.Forms.Button buttonPwd;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}

