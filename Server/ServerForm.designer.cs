namespace ChatTCP.Server
{
    partial class ServerForm
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
            this.gboxSettings = new System.Windows.Forms.GroupBox();
            this.txtPortaTcp = new System.Windows.Forms.TextBox();
            this.lblPortaTcp = new System.Windows.Forms.Label();
            this.cmdStopListening = new System.Windows.Forms.Button();
            this.cmdListen = new System.Windows.Forms.Button();
            this.gboxDatiTx = new System.Windows.Forms.GroupBox();
            this.cmdSend = new System.Windows.Forms.Button();
            this.txtDatiTx = new System.Windows.Forms.TextBox();
            this.cmdDisconnect = new System.Windows.Forms.Button();
            this.gboxDatiRx = new System.Windows.Forms.GroupBox();
            this.txtDatiRx = new System.Windows.Forms.TextBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.gboxSettings.SuspendLayout();
            this.gboxDatiTx.SuspendLayout();
            this.gboxDatiRx.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxSettings
            // 
            this.gboxSettings.Controls.Add(this.txtPortaTcp);
            this.gboxSettings.Controls.Add(this.lblPortaTcp);
            this.gboxSettings.Location = new System.Drawing.Point(12, 12);
            this.gboxSettings.Name = "gboxSettings";
            this.gboxSettings.Size = new System.Drawing.Size(162, 54);
            this.gboxSettings.TabIndex = 1;
            this.gboxSettings.TabStop = false;
            this.gboxSettings.Text = "Impostazioni";
            // 
            // txtPortaTcp
            // 
            this.txtPortaTcp.Location = new System.Drawing.Point(110, 16);
            this.txtPortaTcp.Name = "txtPortaTcp";
            this.txtPortaTcp.Size = new System.Drawing.Size(40, 20);
            this.txtPortaTcp.TabIndex = 1;
            // 
            // lblPortaTcp
            // 
            this.lblPortaTcp.Location = new System.Drawing.Point(13, 20);
            this.lblPortaTcp.Name = "lblPortaTcp";
            this.lblPortaTcp.Size = new System.Drawing.Size(72, 16);
            this.lblPortaTcp.TabIndex = 0;
            this.lblPortaTcp.Text = "Porta TCP:";
            // 
            // cmdStopListening
            // 
            this.cmdStopListening.Location = new System.Drawing.Point(103, 72);
            this.cmdStopListening.Name = "cmdStopListening";
            this.cmdStopListening.Size = new System.Drawing.Size(82, 24);
            this.cmdStopListening.TabIndex = 3;
            this.cmdStopListening.Text = "Stop Listening";
            this.cmdStopListening.Click += new System.EventHandler(this.cmdStopListening_Click);
            // 
            // cmdListen
            // 
            this.cmdListen.Location = new System.Drawing.Point(12, 72);
            this.cmdListen.Name = "cmdListen";
            this.cmdListen.Size = new System.Drawing.Size(85, 24);
            this.cmdListen.TabIndex = 2;
            this.cmdListen.Text = "Start Listening";
            this.cmdListen.Click += new System.EventHandler(this.cmdListen_Click);
            // 
            // gboxDatiTx
            // 
            this.gboxDatiTx.Controls.Add(this.cmdSend);
            this.gboxDatiTx.Controls.Add(this.txtDatiTx);
            this.gboxDatiTx.Location = new System.Drawing.Point(12, 120);
            this.gboxDatiTx.Name = "gboxDatiTx";
            this.gboxDatiTx.Size = new System.Drawing.Size(162, 152);
            this.gboxDatiTx.TabIndex = 4;
            this.gboxDatiTx.TabStop = false;
            this.gboxDatiTx.Text = "Dati Tx";
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(16, 120);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(48, 24);
            this.cmdSend.TabIndex = 1;
            this.cmdSend.Text = "Send";
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // txtDatiTx
            // 
            this.txtDatiTx.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDatiTx.Location = new System.Drawing.Point(8, 16);
            this.txtDatiTx.Multiline = true;
            this.txtDatiTx.Name = "txtDatiTx";
            this.txtDatiTx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDatiTx.Size = new System.Drawing.Size(142, 96);
            this.txtDatiTx.TabIndex = 0;
            // 
            // cmdDisconnect
            // 
            this.cmdDisconnect.Location = new System.Drawing.Point(15, 386);
            this.cmdDisconnect.Name = "cmdDisconnect";
            this.cmdDisconnect.Size = new System.Drawing.Size(82, 24);
            this.cmdDisconnect.TabIndex = 2;
            this.cmdDisconnect.Text = "Disconnect";
            this.cmdDisconnect.Click += new System.EventHandler(this.cmdDisconnect_Click);
            // 
            // gboxDatiRx
            // 
            this.gboxDatiRx.Controls.Add(this.txtDatiRx);
            this.gboxDatiRx.Location = new System.Drawing.Point(12, 278);
            this.gboxDatiRx.Name = "gboxDatiRx";
            this.gboxDatiRx.Size = new System.Drawing.Size(162, 102);
            this.gboxDatiRx.TabIndex = 5;
            this.gboxDatiRx.TabStop = false;
            this.gboxDatiRx.Text = "Dati Rx";
            // 
            // txtDatiRx
            // 
            this.txtDatiRx.Location = new System.Drawing.Point(6, 19);
            this.txtDatiRx.Multiline = true;
            this.txtDatiRx.Name = "txtDatiRx";
            this.txtDatiRx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDatiRx.Size = new System.Drawing.Size(144, 70);
            this.txtDatiRx.TabIndex = 2;
            // 
            // lstLog
            // 
            this.lstLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLog.FormattingEnabled = true;
            this.lstLog.ItemHeight = 14;
            this.lstLog.Location = new System.Drawing.Point(191, 12);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(388, 396);
            this.lstLog.TabIndex = 6;
            // 
            // frmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 422);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.gboxDatiRx);
            this.Controls.Add(this.cmdDisconnect);
            this.Controls.Add(this.gboxDatiTx);
            this.Controls.Add(this.cmdStopListening);
            this.Controls.Add(this.gboxSettings);
            this.Controls.Add(this.cmdListen);
            this.Name = "frmServer";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.frmServer_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServer_FormClosing);
            this.gboxSettings.ResumeLayout(false);
            this.gboxSettings.PerformLayout();
            this.gboxDatiTx.ResumeLayout(false);
            this.gboxDatiTx.PerformLayout();
            this.gboxDatiRx.ResumeLayout(false);
            this.gboxDatiRx.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxSettings;
        private System.Windows.Forms.TextBox txtPortaTcp;
        private System.Windows.Forms.Label lblPortaTcp;
        private System.Windows.Forms.Button cmdStopListening;
        private System.Windows.Forms.Button cmdListen;
        private System.Windows.Forms.GroupBox gboxDatiTx;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.TextBox txtDatiTx;
        private System.Windows.Forms.Button cmdDisconnect;
        private System.Windows.Forms.GroupBox gboxDatiRx;
        private System.Windows.Forms.TextBox txtDatiRx;
        private System.Windows.Forms.ListBox lstLog;

    
    
    
    
    }  // Classe
}  // Namespace

