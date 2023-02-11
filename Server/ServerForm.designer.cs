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
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.PortaTcpTextBox = new System.Windows.Forms.TextBox();
            this.PortaTcpLabel = new System.Windows.Forms.Label();
            this.StopListeningButton = new System.Windows.Forms.Button();
            this.StartListeningButton = new System.Windows.Forms.Button();
            this.DatiTxGroupBox = new System.Windows.Forms.GroupBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.DatiTxTextBox = new System.Windows.Forms.TextBox();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.DatiRxGroupBox = new System.Windows.Forms.GroupBox();
            this.DatiRxTextBox = new System.Windows.Forms.TextBox();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.SettingsGroupBox.SuspendLayout();
            this.DatiTxGroupBox.SuspendLayout();
            this.DatiRxGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.PortaTcpTextBox);
            this.SettingsGroupBox.Controls.Add(this.PortaTcpLabel);
            this.SettingsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(162, 54);
            this.SettingsGroupBox.TabIndex = 1;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Impostazioni";
            // 
            // PortaTcpTextBox
            // 
            this.PortaTcpTextBox.Location = new System.Drawing.Point(110, 16);
            this.PortaTcpTextBox.Name = "PortaTcpTextBox";
            this.PortaTcpTextBox.Size = new System.Drawing.Size(40, 20);
            this.PortaTcpTextBox.TabIndex = 1;
            // 
            // PortaTcpLabel
            // 
            this.PortaTcpLabel.Location = new System.Drawing.Point(13, 20);
            this.PortaTcpLabel.Name = "PortaTcpLabel";
            this.PortaTcpLabel.Size = new System.Drawing.Size(72, 16);
            this.PortaTcpLabel.TabIndex = 0;
            this.PortaTcpLabel.Text = "Porta TCP:";
            // 
            // StopListeningButton
            // 
            this.StopListeningButton.Location = new System.Drawing.Point(103, 72);
            this.StopListeningButton.Name = "StopListeningButton";
            this.StopListeningButton.Size = new System.Drawing.Size(82, 24);
            this.StopListeningButton.TabIndex = 3;
            this.StopListeningButton.Text = "Stop Listening";
            this.StopListeningButton.Click += new System.EventHandler(this.StopListeningButton_Click);
            // 
            // StartListeningButton
            // 
            this.StartListeningButton.Location = new System.Drawing.Point(12, 72);
            this.StartListeningButton.Name = "StartListeningButton";
            this.StartListeningButton.Size = new System.Drawing.Size(85, 24);
            this.StartListeningButton.TabIndex = 2;
            this.StartListeningButton.Text = "Start Listening";
            this.StartListeningButton.Click += new System.EventHandler(this.StartListeningButton_Click);
            // 
            // DatiTxGroupBox
            // 
            this.DatiTxGroupBox.Controls.Add(this.SendButton);
            this.DatiTxGroupBox.Controls.Add(this.DatiTxTextBox);
            this.DatiTxGroupBox.Location = new System.Drawing.Point(12, 120);
            this.DatiTxGroupBox.Name = "DatiTxGroupBox";
            this.DatiTxGroupBox.Size = new System.Drawing.Size(162, 152);
            this.DatiTxGroupBox.TabIndex = 4;
            this.DatiTxGroupBox.TabStop = false;
            this.DatiTxGroupBox.Text = "Dati Tx";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(16, 120);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(48, 24);
            this.SendButton.TabIndex = 1;
            this.SendButton.Text = "Send";
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // DatiTxTextBox
            // 
            this.DatiTxTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.DatiTxTextBox.Location = new System.Drawing.Point(8, 16);
            this.DatiTxTextBox.Multiline = true;
            this.DatiTxTextBox.Name = "DatiTxTextBox";
            this.DatiTxTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DatiTxTextBox.Size = new System.Drawing.Size(142, 96);
            this.DatiTxTextBox.TabIndex = 0;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(15, 386);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(82, 24);
            this.DisconnectButton.TabIndex = 2;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // DatiRxGroupBox
            // 
            this.DatiRxGroupBox.Controls.Add(this.DatiRxTextBox);
            this.DatiRxGroupBox.Location = new System.Drawing.Point(12, 278);
            this.DatiRxGroupBox.Name = "DatiRxGroupBox";
            this.DatiRxGroupBox.Size = new System.Drawing.Size(162, 102);
            this.DatiRxGroupBox.TabIndex = 5;
            this.DatiRxGroupBox.TabStop = false;
            this.DatiRxGroupBox.Text = "Dati Rx";
            // 
            // DatiRxTextBox
            // 
            this.DatiRxTextBox.Location = new System.Drawing.Point(6, 19);
            this.DatiRxTextBox.Multiline = true;
            this.DatiRxTextBox.Name = "DatiRxTextBox";
            this.DatiRxTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DatiRxTextBox.Size = new System.Drawing.Size(144, 70);
            this.DatiRxTextBox.TabIndex = 2;
            // 
            // LogListBox
            // 
            this.LogListBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.ItemHeight = 14;
            this.LogListBox.Location = new System.Drawing.Point(191, 12);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(388, 396);
            this.LogListBox.TabIndex = 6;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 422);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.DatiRxGroupBox);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.DatiTxGroupBox);
            this.Controls.Add(this.StopListeningButton);
            this.Controls.Add(this.SettingsGroupBox);
            this.Controls.Add(this.StartListeningButton);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.DatiTxGroupBox.ResumeLayout(false);
            this.DatiTxGroupBox.PerformLayout();
            this.DatiRxGroupBox.ResumeLayout(false);
            this.DatiRxGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.TextBox PortaTcpTextBox;
        private System.Windows.Forms.Label PortaTcpLabel;
        private System.Windows.Forms.Button StopListeningButton;
        private System.Windows.Forms.Button StartListeningButton;
        private System.Windows.Forms.GroupBox DatiTxGroupBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox DatiTxTextBox;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.GroupBox DatiRxGroupBox;
        private System.Windows.Forms.TextBox DatiRxTextBox;
        private System.Windows.Forms.ListBox LogListBox;

    
    
    
    
    }  // Classe
}  // Namespace

