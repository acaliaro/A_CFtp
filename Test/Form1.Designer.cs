namespace Test
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.buttonConnetti = new System.Windows.Forms.Button();
            this.buttonEsci = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.buttonList = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonGet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Server FTP:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(100, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 23);
            this.textBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.Text = "username:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.Text = "password:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.Text = "port:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(100, 28);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(132, 23);
            this.textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(100, 52);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(132, 23);
            this.textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(100, 76);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(132, 23);
            this.textBox4.TabIndex = 10;
            this.textBox4.Text = "21";
            // 
            // buttonConnetti
            // 
            this.buttonConnetti.Location = new System.Drawing.Point(156, 104);
            this.buttonConnetti.Name = "buttonConnetti";
            this.buttonConnetti.Size = new System.Drawing.Size(72, 20);
            this.buttonConnetti.TabIndex = 11;
            this.buttonConnetti.Text = "Connetti";
            this.buttonConnetti.Click += new System.EventHandler(this.click);
            // 
            // buttonEsci
            // 
            this.buttonEsci.Location = new System.Drawing.Point(80, 104);
            this.buttonEsci.Name = "buttonEsci";
            this.buttonEsci.Size = new System.Drawing.Size(72, 20);
            this.buttonEsci.TabIndex = 12;
            this.buttonEsci.Text = "Esci";
            this.buttonEsci.Click += new System.EventHandler(this.click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(8, 128);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(224, 100);
            this.textBox5.TabIndex = 13;
            // 
            // buttonList
            // 
            this.buttonList.Location = new System.Drawing.Point(12, 236);
            this.buttonList.Name = "buttonList";
            this.buttonList.Size = new System.Drawing.Size(72, 20);
            this.buttonList.TabIndex = 14;
            this.buttonList.Text = "List";
            this.buttonList.Click += new System.EventHandler(this.click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(88, 236);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(72, 20);
            this.buttonSend.TabIndex = 15;
            this.buttonSend.Text = "Send";
            this.buttonSend.Click += new System.EventHandler(this.click);
            // 
            // buttonGet
            // 
            this.buttonGet.Location = new System.Drawing.Point(164, 236);
            this.buttonGet.Name = "buttonGet";
            this.buttonGet.Size = new System.Drawing.Size(72, 20);
            this.buttonGet.TabIndex = 16;
            this.buttonGet.Text = "Get";
            this.buttonGet.Click += new System.EventHandler(this.click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.buttonGet);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonList);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.buttonEsci);
            this.Controls.Add(this.buttonConnetti);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button buttonConnetti;
        private System.Windows.Forms.Button buttonEsci;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button buttonList;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonGet;
    }
}