namespace ChatTCP.Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gboxImpostazioni = new System.Windows.Forms.GroupBox();
            this.cmbNetworkComputers = new System.Windows.Forms.ComboBox();
            this.txtPortaTcp = new System.Windows.Forms.TextBox();
            this.lblPortaTcp = new System.Windows.Forms.Label();
            this.txtIPRemoto = new System.Windows.Forms.TextBox();
            this.lblIpRemoto = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.gboxDatiTx = new System.Windows.Forms.GroupBox();
            this.cmdSend = new System.Windows.Forms.Button();
            this.txtDatiTx = new System.Windows.Forms.TextBox();
            this.gboxDatiRx = new System.Windows.Forms.GroupBox();
            this.txtDatiRx = new System.Windows.Forms.TextBox();
            this.gboxImpostazioni.SuspendLayout();
            this.gboxDatiTx.SuspendLayout();
            this.gboxDatiRx.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxImpostazioni
            // 
            this.gboxImpostazioni.Controls.Add(this.cmbNetworkComputers);
            this.gboxImpostazioni.Controls.Add(this.txtPortaTcp);
            this.gboxImpostazioni.Controls.Add(this.lblPortaTcp);
            this.gboxImpostazioni.Controls.Add(this.txtIPRemoto);
            this.gboxImpostazioni.Controls.Add(this.lblIpRemoto);
            this.gboxImpostazioni.Location = new System.Drawing.Point(12, 2);
            this.gboxImpostazioni.Name = "gboxImpostazioni";
            this.gboxImpostazioni.Size = new System.Drawing.Size(161, 111);
            this.gboxImpostazioni.TabIndex = 1;
            this.gboxImpostazioni.TabStop = false;
            this.gboxImpostazioni.Text = "Impostazioni";
            // 
            // cmbNetworkComputers
            // 
            this.cmbNetworkComputers.FormattingEnabled = true;
            this.cmbNetworkComputers.Location = new System.Drawing.Point(8, 61);
            this.cmbNetworkComputers.Name = "cmbNetworkComputers";
            this.cmbNetworkComputers.Size = new System.Drawing.Size(147, 21);
            this.cmbNetworkComputers.TabIndex = 4;
            this.cmbNetworkComputers.SelectedIndexChanged += new System.EventHandler(this.cmbNetworkComputers_SelectedIndexChanged);
            // 
            // txtPortaTcp
            // 
            this.txtPortaTcp.Location = new System.Drawing.Point(72, 86);
            this.txtPortaTcp.Name = "txtPortaTcp";
            this.txtPortaTcp.Size = new System.Drawing.Size(40, 20);
            this.txtPortaTcp.TabIndex = 3;
            this.txtPortaTcp.Text = "8221";
            // 
            // lblPortaTcp
            // 
            this.lblPortaTcp.Location = new System.Drawing.Point(6, 89);
            this.lblPortaTcp.Name = "lblPortaTcp";
            this.lblPortaTcp.Size = new System.Drawing.Size(60, 20);
            this.lblPortaTcp.TabIndex = 2;
            this.lblPortaTcp.Text = "Porta TCP:";
            // 
            // txtIPRemoto
            // 
            this.txtIPRemoto.Location = new System.Drawing.Point(11, 37);
            this.txtIPRemoto.Name = "txtIPRemoto";
            this.txtIPRemoto.Size = new System.Drawing.Size(142, 20);
            this.txtIPRemoto.TabIndex = 1;
            this.txtIPRemoto.Text = "127.0.0.1";
            this.txtIPRemoto.Visible = false;
            // 
            // lblIpRemoto
            // 
            this.lblIpRemoto.Location = new System.Drawing.Point(6, 22);
            this.lblIpRemoto.Name = "lblIpRemoto";
            this.lblIpRemoto.Size = new System.Drawing.Size(60, 16);
            this.lblIpRemoto.TabIndex = 0;
            this.lblIpRemoto.Text = "IP Remoto:";
            // 
            // cmdClose
            // 
            this.cmdClose.Enabled = false;
            this.cmdClose.Location = new System.Drawing.Point(116, 117);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(57, 24);
            this.cmdClose.TabIndex = 5;
            this.cmdClose.Text = "Close";
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(12, 117);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(57, 24);
            this.cmdConnect.TabIndex = 4;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // lstLog
            // 
            this.lstLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLog.FormattingEnabled = true;
            this.lstLog.ItemHeight = 14;
            this.lstLog.Location = new System.Drawing.Point(179, 12);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(393, 396);
            this.lstLog.TabIndex = 7;
            // 
            // gboxDatiTx
            // 
            this.gboxDatiTx.Controls.Add(this.cmdSend);
            this.gboxDatiTx.Controls.Add(this.txtDatiTx);
            this.gboxDatiTx.Location = new System.Drawing.Point(12, 152);
            this.gboxDatiTx.Name = "gboxDatiTx";
            this.gboxDatiTx.Size = new System.Drawing.Size(161, 152);
            this.gboxDatiTx.TabIndex = 8;
            this.gboxDatiTx.TabStop = false;
            this.gboxDatiTx.Text = "Dati Tx";
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(16, 120);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(50, 24);
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
            this.txtDatiTx.Size = new System.Drawing.Size(144, 96);
            this.txtDatiTx.TabIndex = 0;
            // 
            // gboxDatiRx
            // 
            this.gboxDatiRx.Controls.Add(this.txtDatiRx);
            this.gboxDatiRx.Location = new System.Drawing.Point(12, 310);
            this.gboxDatiRx.Name = "gboxDatiRx";
            this.gboxDatiRx.Size = new System.Drawing.Size(161, 102);
            this.gboxDatiRx.TabIndex = 9;
            this.gboxDatiRx.TabStop = false;
            this.gboxDatiRx.Text = "Dati Rx";
            // 
            // txtDatiRx
            // 
            this.txtDatiRx.Location = new System.Drawing.Point(8, 19);
            this.txtDatiRx.Multiline = true;
            this.txtDatiRx.Name = "txtDatiRx";
            this.txtDatiRx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDatiRx.Size = new System.Drawing.Size(144, 70);
            this.txtDatiRx.TabIndex = 2;
            // 
            // frmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 423);
            this.Controls.Add(this.gboxDatiRx);
            this.Controls.Add(this.gboxDatiTx);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.gboxImpostazioni);
            this.Name = "frmClient";
            this.Text = "frmClient";
            this.Load += new System.EventHandler(this.frmClient_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmClient_FormClosing);
            this.gboxImpostazioni.ResumeLayout(false);
            this.gboxImpostazioni.PerformLayout();
            this.gboxDatiTx.ResumeLayout(false);
            this.gboxDatiTx.PerformLayout();
            this.gboxDatiRx.ResumeLayout(false);
            this.gboxDatiRx.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxImpostazioni;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.TextBox txtPortaTcp;
        private System.Windows.Forms.Label lblPortaTcp;
        private System.Windows.Forms.TextBox txtIPRemoto;
        private System.Windows.Forms.Label lblIpRemoto;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.GroupBox gboxDatiTx;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.TextBox txtDatiTx;
        private System.Windows.Forms.GroupBox gboxDatiRx;
        private System.Windows.Forms.TextBox txtDatiRx;
        private System.Windows.Forms.ComboBox cmbNetworkComputers;
    }
}