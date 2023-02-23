namespace ChatTCP.Client
{
    partial class FormUserInfo
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
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.AccountGroupBox = new System.Windows.Forms.GroupBox();
            this.ChangePasswordButton = new System.Windows.Forms.Button();
            this.InformazioniGroupBox = new System.Windows.Forms.GroupBox();
            this.CognomeTextBox = new System.Windows.Forms.TextBox();
            this.CognomeLabel = new System.Windows.Forms.Label();
            this.NomeTextBox = new System.Windows.Forms.TextBox();
            this.NomeLabel = new System.Windows.Forms.Label();
            this.UpdateUserInfoButton = new System.Windows.Forms.Button();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.AccountGroupBox.SuspendLayout();
            this.InformazioniGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(6, 16);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(107, 13);
            this.UsernameLabel.TabIndex = 1;
            this.UsernameLabel.Text = "Username: username";
            // 
            // AccountGroupBox
            // 
            this.AccountGroupBox.Controls.Add(this.ChangePasswordButton);
            this.AccountGroupBox.Controls.Add(this.UsernameLabel);
            this.AccountGroupBox.Location = new System.Drawing.Point(12, 12);
            this.AccountGroupBox.Name = "AccountGroupBox";
            this.AccountGroupBox.Size = new System.Drawing.Size(278, 66);
            this.AccountGroupBox.TabIndex = 2;
            this.AccountGroupBox.TabStop = false;
            this.AccountGroupBox.Text = "Account";
            // 
            // ChangePasswordButton
            // 
            this.ChangePasswordButton.Location = new System.Drawing.Point(6, 32);
            this.ChangePasswordButton.Name = "ChangePasswordButton";
            this.ChangePasswordButton.Size = new System.Drawing.Size(106, 23);
            this.ChangePasswordButton.TabIndex = 2;
            this.ChangePasswordButton.Text = "Cambia password";
            this.ChangePasswordButton.UseVisualStyleBackColor = true;
            this.ChangePasswordButton.Click += new System.EventHandler(this.ChangePasswordButton_Click);
            // 
            // InformazioniGroupBox
            // 
            this.InformazioniGroupBox.Controls.Add(this.EmailTextBox);
            this.InformazioniGroupBox.Controls.Add(this.EmailLabel);
            this.InformazioniGroupBox.Controls.Add(this.CognomeTextBox);
            this.InformazioniGroupBox.Controls.Add(this.CognomeLabel);
            this.InformazioniGroupBox.Controls.Add(this.NomeTextBox);
            this.InformazioniGroupBox.Controls.Add(this.NomeLabel);
            this.InformazioniGroupBox.Controls.Add(this.UpdateUserInfoButton);
            this.InformazioniGroupBox.Location = new System.Drawing.Point(12, 84);
            this.InformazioniGroupBox.Name = "InformazioniGroupBox";
            this.InformazioniGroupBox.Size = new System.Drawing.Size(278, 133);
            this.InformazioniGroupBox.TabIndex = 3;
            this.InformazioniGroupBox.TabStop = false;
            this.InformazioniGroupBox.Text = "Informazioni";
            // 
            // CognomeTextBox
            // 
            this.CognomeTextBox.Location = new System.Drawing.Point(64, 48);
            this.CognomeTextBox.Name = "CognomeTextBox";
            this.CognomeTextBox.Size = new System.Drawing.Size(208, 20);
            this.CognomeTextBox.TabIndex = 5;
            // 
            // CognomeLabel
            // 
            this.CognomeLabel.AutoSize = true;
            this.CognomeLabel.Location = new System.Drawing.Point(6, 51);
            this.CognomeLabel.Name = "CognomeLabel";
            this.CognomeLabel.Size = new System.Drawing.Size(52, 13);
            this.CognomeLabel.TabIndex = 4;
            this.CognomeLabel.Text = "Cognome";
            // 
            // NomeTextBox
            // 
            this.NomeTextBox.Location = new System.Drawing.Point(64, 20);
            this.NomeTextBox.Name = "NomeTextBox";
            this.NomeTextBox.Size = new System.Drawing.Size(208, 20);
            this.NomeTextBox.TabIndex = 3;
            // 
            // NomeLabel
            // 
            this.NomeLabel.AutoSize = true;
            this.NomeLabel.Location = new System.Drawing.Point(6, 23);
            this.NomeLabel.Name = "NomeLabel";
            this.NomeLabel.Size = new System.Drawing.Size(35, 13);
            this.NomeLabel.TabIndex = 2;
            this.NomeLabel.Text = "Nome";
            // 
            // UpdateUserInfoButton
            // 
            this.UpdateUserInfoButton.Location = new System.Drawing.Point(6, 101);
            this.UpdateUserInfoButton.Name = "UpdateUserInfoButton";
            this.UpdateUserInfoButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateUserInfoButton.TabIndex = 1;
            this.UpdateUserInfoButton.Text = "Aggiorna";
            this.UpdateUserInfoButton.UseVisualStyleBackColor = true;
            this.UpdateUserInfoButton.Click += new System.EventHandler(this.UpdateUserInfoButton_Click);
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(6, 78);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(32, 13);
            this.EmailLabel.TabIndex = 6;
            this.EmailLabel.Text = "Email";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Location = new System.Drawing.Point(64, 75);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(208, 20);
            this.EmailTextBox.TabIndex = 7;
            // 
            // FormUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 226);
            this.Controls.Add(this.InformazioniGroupBox);
            this.Controls.Add(this.AccountGroupBox);
            this.Name = "FormUserInfo";
            this.Text = "Informazioni utente";
            this.Load += new System.EventHandler(this.FormUserInfo_Load);
            this.AccountGroupBox.ResumeLayout(false);
            this.AccountGroupBox.PerformLayout();
            this.InformazioniGroupBox.ResumeLayout(false);
            this.InformazioniGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.GroupBox AccountGroupBox;
        private System.Windows.Forms.Button ChangePasswordButton;
        private System.Windows.Forms.GroupBox InformazioniGroupBox;
        private System.Windows.Forms.Button UpdateUserInfoButton;
        private System.Windows.Forms.Label CognomeLabel;
        private System.Windows.Forms.TextBox NomeTextBox;
        private System.Windows.Forms.Label NomeLabel;
        private System.Windows.Forms.TextBox CognomeTextBox;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Label EmailLabel;
    }
}