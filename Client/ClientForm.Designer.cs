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
            this.ImpostazioniGroupBox = new System.Windows.Forms.GroupBox();
            this.NetworkComputersComboBox = new System.Windows.Forms.ComboBox();
            this.PortaTcpTextBox = new System.Windows.Forms.TextBox();
            this.PortaTcpLabel = new System.Windows.Forms.Label();
            this.IpRemotoLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.DatiTxGroupBox = new System.Windows.Forms.GroupBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.DatiTxTextBox = new System.Windows.Forms.TextBox();
            this.DatiRxGroupBox = new System.Windows.Forms.GroupBox();
            this.DatiRxTextBox = new System.Windows.Forms.TextBox();
            this.ImpostazioniGroupBox.SuspendLayout();
            this.DatiTxGroupBox.SuspendLayout();
            this.DatiRxGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImpostazioniGroupBox
            // 
            this.ImpostazioniGroupBox.Controls.Add(this.NetworkComputersComboBox);
            this.ImpostazioniGroupBox.Controls.Add(this.PortaTcpTextBox);
            this.ImpostazioniGroupBox.Controls.Add(this.PortaTcpLabel);
            this.ImpostazioniGroupBox.Controls.Add(this.IpRemotoLabel);
            this.ImpostazioniGroupBox.Location = new System.Drawing.Point(12, 2);
            this.ImpostazioniGroupBox.Name = "ImpostazioniGroupBox";
            this.ImpostazioniGroupBox.Size = new System.Drawing.Size(161, 97);
            this.ImpostazioniGroupBox.TabIndex = 1;
            this.ImpostazioniGroupBox.TabStop = false;
            this.ImpostazioniGroupBox.Text = "Impostazioni";
            // 
            // NetworkComputersComboBox
            // 
            this.NetworkComputersComboBox.FormattingEnabled = true;
            this.NetworkComputersComboBox.Location = new System.Drawing.Point(8, 41);
            this.NetworkComputersComboBox.Name = "NetworkComputersComboBox";
            this.NetworkComputersComboBox.Size = new System.Drawing.Size(147, 21);
            this.NetworkComputersComboBox.TabIndex = 4;
            // 
            // PortaTcpTextBox
            // 
            this.PortaTcpTextBox.Location = new System.Drawing.Point(71, 68);
            this.PortaTcpTextBox.Name = "PortaTcpTextBox";
            this.PortaTcpTextBox.Size = new System.Drawing.Size(40, 20);
            this.PortaTcpTextBox.TabIndex = 3;
            this.PortaTcpTextBox.Text = "8221";
            // 
            // PortaTcpLabel
            // 
            this.PortaTcpLabel.Location = new System.Drawing.Point(5, 71);
            this.PortaTcpLabel.Name = "PortaTcpLabel";
            this.PortaTcpLabel.Size = new System.Drawing.Size(60, 20);
            this.PortaTcpLabel.TabIndex = 2;
            this.PortaTcpLabel.Text = "Porta TCP:";
            // 
            // IpRemotoLabel
            // 
            this.IpRemotoLabel.Location = new System.Drawing.Point(6, 22);
            this.IpRemotoLabel.Name = "IpRemotoLabel";
            this.IpRemotoLabel.Size = new System.Drawing.Size(60, 16);
            this.IpRemotoLabel.TabIndex = 0;
            this.IpRemotoLabel.Text = "IP Remoto:";
            // 
            // CloseButton
            // 
            this.CloseButton.Enabled = false;
            this.CloseButton.Location = new System.Drawing.Point(116, 105);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(57, 24);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(12, 105);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(57, 24);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // LogListBox
            // 
            this.LogListBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.ItemHeight = 14;
            this.LogListBox.Location = new System.Drawing.Point(179, 12);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(393, 396);
            this.LogListBox.TabIndex = 7;
            // 
            // DatiTxGroupBox
            // 
            this.DatiTxGroupBox.Controls.Add(this.SendButton);
            this.DatiTxGroupBox.Controls.Add(this.DatiTxTextBox);
            this.DatiTxGroupBox.Location = new System.Drawing.Point(12, 135);
            this.DatiTxGroupBox.Name = "DatiTxGroupBox";
            this.DatiTxGroupBox.Size = new System.Drawing.Size(161, 152);
            this.DatiTxGroupBox.TabIndex = 8;
            this.DatiTxGroupBox.TabStop = false;
            this.DatiTxGroupBox.Text = "Dati Tx";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(16, 120);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(50, 24);
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
            this.DatiTxTextBox.Size = new System.Drawing.Size(144, 96);
            this.DatiTxTextBox.TabIndex = 0;
            // 
            // DatiRxGroupBox
            // 
            this.DatiRxGroupBox.Controls.Add(this.DatiRxTextBox);
            this.DatiRxGroupBox.Location = new System.Drawing.Point(12, 293);
            this.DatiRxGroupBox.Name = "DatiRxGroupBox";
            this.DatiRxGroupBox.Size = new System.Drawing.Size(161, 119);
            this.DatiRxGroupBox.TabIndex = 9;
            this.DatiRxGroupBox.TabStop = false;
            this.DatiRxGroupBox.Text = "Dati Rx";
            // 
            // DatiRxTextBox
            // 
            this.DatiRxTextBox.Location = new System.Drawing.Point(8, 19);
            this.DatiRxTextBox.Multiline = true;
            this.DatiRxTextBox.Name = "DatiRxTextBox";
            this.DatiRxTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DatiRxTextBox.Size = new System.Drawing.Size(144, 94);
            this.DatiRxTextBox.TabIndex = 2;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 423);
            this.Controls.Add(this.DatiRxGroupBox);
            this.Controls.Add(this.DatiTxGroupBox);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ImpostazioniGroupBox);
            this.Name = "ClientForm";
            this.Text = "frmClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ImpostazioniGroupBox.ResumeLayout(false);
            this.ImpostazioniGroupBox.PerformLayout();
            this.DatiTxGroupBox.ResumeLayout(false);
            this.DatiTxGroupBox.PerformLayout();
            this.DatiRxGroupBox.ResumeLayout(false);
            this.DatiRxGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ImpostazioniGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox PortaTcpTextBox;
        private System.Windows.Forms.Label PortaTcpLabel;
        private System.Windows.Forms.Label IpRemotoLabel;
        private System.Windows.Forms.ListBox LogListBox;
        private System.Windows.Forms.GroupBox DatiTxGroupBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox DatiTxTextBox;
        private System.Windows.Forms.GroupBox DatiRxGroupBox;
        private System.Windows.Forms.TextBox DatiRxTextBox;
        private System.Windows.Forms.ComboBox NetworkComputersComboBox;
    }
}