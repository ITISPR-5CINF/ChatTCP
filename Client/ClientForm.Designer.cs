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
            this.CloseButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.IpRemotoLabel = new System.Windows.Forms.Label();
            this.MessagesListBox = new System.Windows.Forms.ListBox();
            this.SendGroupBox = new System.Windows.Forms.GroupBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.SendTextBox = new System.Windows.Forms.TextBox();
            this.LoggingGroupBox = new System.Windows.Forms.GroupBox();
            this.LoggingListBox = new System.Windows.Forms.ListBox();
            this.ImpostazioniGroupBox.SuspendLayout();
            this.SendGroupBox.SuspendLayout();
            this.LoggingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImpostazioniGroupBox
            // 
            this.ImpostazioniGroupBox.Controls.Add(this.NetworkComputersComboBox);
            this.ImpostazioniGroupBox.Controls.Add(this.PortaTcpTextBox);
            this.ImpostazioniGroupBox.Controls.Add(this.PortaTcpLabel);
            this.ImpostazioniGroupBox.Controls.Add(this.IpRemotoLabel);
            this.ImpostazioniGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ImpostazioniGroupBox.Name = "ImpostazioniGroupBox";
            this.ImpostazioniGroupBox.Size = new System.Drawing.Size(161, 93);
            this.ImpostazioniGroupBox.TabIndex = 1;
            this.ImpostazioniGroupBox.TabStop = false;
            this.ImpostazioniGroupBox.Text = "Impostazioni";
            // 
            // NetworkComputersComboBox
            // 
            this.NetworkComputersComboBox.FormattingEnabled = true;
            this.NetworkComputersComboBox.Location = new System.Drawing.Point(6, 35);
            this.NetworkComputersComboBox.Name = "NetworkComputersComboBox";
            this.NetworkComputersComboBox.Size = new System.Drawing.Size(149, 21);
            this.NetworkComputersComboBox.TabIndex = 4;
            // 
            // PortaTcpTextBox
            // 
            this.PortaTcpTextBox.Location = new System.Drawing.Point(72, 62);
            this.PortaTcpTextBox.Name = "PortaTcpTextBox";
            this.PortaTcpTextBox.Size = new System.Drawing.Size(40, 20);
            this.PortaTcpTextBox.TabIndex = 3;
            this.PortaTcpTextBox.Text = "8221";
            // 
            // PortaTcpLabel
            // 
            this.PortaTcpLabel.Location = new System.Drawing.Point(6, 65);
            this.PortaTcpLabel.Name = "PortaTcpLabel";
            this.PortaTcpLabel.Size = new System.Drawing.Size(60, 20);
            this.PortaTcpLabel.TabIndex = 2;
            this.PortaTcpLabel.Text = "Porta TCP:";
            // 
            // CloseButton
            // 
            this.CloseButton.Enabled = false;
            this.CloseButton.Location = new System.Drawing.Point(116, 111);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(57, 24);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(12, 111);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(57, 24);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // IpRemotoLabel
            // 
            this.IpRemotoLabel.Location = new System.Drawing.Point(6, 16);
            this.IpRemotoLabel.Name = "IpRemotoLabel";
            this.IpRemotoLabel.Size = new System.Drawing.Size(60, 16);
            this.IpRemotoLabel.TabIndex = 0;
            this.IpRemotoLabel.Text = "IP Remoto:";
            // 
            // MessagesListBox
            // 
            this.MessagesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessagesListBox.FormattingEnabled = true;
            this.MessagesListBox.Location = new System.Drawing.Point(6, 19);
            this.MessagesListBox.Name = "MessagesListBox";
            this.MessagesListBox.Size = new System.Drawing.Size(379, 342);
            this.MessagesListBox.TabIndex = 7;
            // 
            // SendGroupBox
            // 
            this.SendGroupBox.Controls.Add(this.SendButton);
            this.SendGroupBox.Controls.Add(this.SendTextBox);
            this.SendGroupBox.Controls.Add(this.MessagesListBox);
            this.SendGroupBox.Location = new System.Drawing.Point(180, 12);
            this.SendGroupBox.Name = "SendGroupBox";
            this.SendGroupBox.Size = new System.Drawing.Size(391, 400);
            this.SendGroupBox.TabIndex = 8;
            this.SendGroupBox.TabStop = false;
            this.SendGroupBox.Text = "Messaggi";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(335, 367);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(50, 25);
            this.SendButton.TabIndex = 1;
            this.SendButton.Text = "Send";
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // SendTextBox
            // 
            this.SendTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SendTextBox.Location = new System.Drawing.Point(6, 370);
            this.SendTextBox.Name = "SendTextBox";
            this.SendTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SendTextBox.Size = new System.Drawing.Size(323, 20);
            this.SendTextBox.TabIndex = 0;
            // 
            // LoggingGroupBox
            // 
            this.LoggingGroupBox.Controls.Add(this.LoggingListBox);
            this.LoggingGroupBox.Location = new System.Drawing.Point(12, 141);
            this.LoggingGroupBox.Name = "LoggingGroupBox";
            this.LoggingGroupBox.Size = new System.Drawing.Size(161, 271);
            this.LoggingGroupBox.TabIndex = 9;
            this.LoggingGroupBox.TabStop = false;
            this.LoggingGroupBox.Text = "Logging";
            // 
            // LoggingListBox
            // 
            this.LoggingListBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.LoggingListBox.FormattingEnabled = true;
            this.LoggingListBox.ItemHeight = 14;
            this.LoggingListBox.Location = new System.Drawing.Point(6, 19);
            this.LoggingListBox.Name = "LoggingListBox";
            this.LoggingListBox.Size = new System.Drawing.Size(149, 242);
            this.LoggingListBox.TabIndex = 0;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 423);
            this.Controls.Add(this.LoggingGroupBox);
            this.Controls.Add(this.SendGroupBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ImpostazioniGroupBox);
            this.Controls.Add(this.ConnectButton);
            this.Name = "ClientForm";
            this.Text = "ChatTCP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ImpostazioniGroupBox.ResumeLayout(false);
            this.ImpostazioniGroupBox.PerformLayout();
            this.SendGroupBox.ResumeLayout(false);
            this.SendGroupBox.PerformLayout();
            this.LoggingGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ImpostazioniGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox PortaTcpTextBox;
        private System.Windows.Forms.Label PortaTcpLabel;
        private System.Windows.Forms.Label IpRemotoLabel;
        private System.Windows.Forms.ListBox MessagesListBox;
        private System.Windows.Forms.GroupBox SendGroupBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox SendTextBox;
        private System.Windows.Forms.GroupBox LoggingGroupBox;
        private System.Windows.Forms.ComboBox NetworkComputersComboBox;
        private System.Windows.Forms.ListBox LoggingListBox;
    }
}