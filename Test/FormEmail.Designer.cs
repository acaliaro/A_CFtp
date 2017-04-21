namespace Test
{
    partial class FormEmail
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
            this.tbPass = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbBody = new System.Windows.Forms.TextBox();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbTo = new System.Windows.Forms.TextBox();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(4, 160);
            this.tbPass.Name = "tbPass";
            this.tbPass.Size = new System.Drawing.Size(148, 23);
            this.tbPass.TabIndex = 20;
            this.tbPass.Text = "ireland";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(4, 136);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(148, 23);
            this.tbUser.TabIndex = 19;
            this.tbUser.Text = "acaliaro@libero.it";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(4, 112);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(148, 23);
            this.tbServer.TabIndex = 18;
            this.tbServer.Text = "smtp.libero.it";
            // 
            // tbBody
            // 
            this.tbBody.Location = new System.Drawing.Point(4, 88);
            this.tbBody.Name = "tbBody";
            this.tbBody.Size = new System.Drawing.Size(148, 23);
            this.tbBody.TabIndex = 17;
            this.tbBody.Text = "fff";
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(4, 64);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(148, 23);
            this.tbSubject.TabIndex = 16;
            this.tbSubject.Text = "sss";
            // 
            // tbTo
            // 
            this.tbTo.Location = new System.Drawing.Point(4, 36);
            this.tbTo.Name = "tbTo";
            this.tbTo.Size = new System.Drawing.Size(148, 23);
            this.tbTo.TabIndex = 15;
            this.tbTo.Text = "alecaliaro@inwind.it; info@mobi-ware.it";
            // 
            // tbFrom
            // 
            this.tbFrom.Location = new System.Drawing.Point(4, 8);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(148, 23);
            this.tbFrom.TabIndex = 14;
            this.tbFrom.Text = "acaliaro@libero.it";
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(8, 188);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(224, 116);
            this.tbLog.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FormEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.tbBody);
            this.Controls.Add(this.tbSubject);
            this.Controls.Add(this.tbTo);
            this.Controls.Add(this.tbFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEmail";
            this.Text = "FormEmail";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TextBox tbPass;
        internal System.Windows.Forms.TextBox tbUser;
        internal System.Windows.Forms.TextBox tbServer;
        internal System.Windows.Forms.TextBox tbBody;
        internal System.Windows.Forms.TextBox tbSubject;
        internal System.Windows.Forms.TextBox tbTo;
        internal System.Windows.Forms.TextBox tbFrom;
        internal System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button button1;
    }
}