namespace ChatTCP.Client
{
    partial class FormSignin
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
            this.lblSignin = new System.Windows.Forms.Label();
            this.NomeLabel = new System.Windows.Forms.Label();
            this.CognomeLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.NomeTextBox = new System.Windows.Forms.TextBox();
            this.CognomeTextBox = new System.Windows.Forms.TextBox();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.SigninButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSignin
            // 
            this.lblSignin.AutoSize = true;
            this.lblSignin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignin.Location = new System.Drawing.Point(12, 9);
            this.lblSignin.Name = "lblSignin";
            this.lblSignin.Size = new System.Drawing.Size(82, 25);
            this.lblSignin.TabIndex = 0;
            this.lblSignin.Text = "Sign-In";
            // 
            // NomeLabel
            // 
            this.NomeLabel.AutoSize = true;
            this.NomeLabel.Location = new System.Drawing.Point(12, 62);
            this.NomeLabel.Name = "NomeLabel";
            this.NomeLabel.Size = new System.Drawing.Size(35, 13);
            this.NomeLabel.TabIndex = 1;
            this.NomeLabel.Text = "Nome";
            // 
            // CognomeLabel
            // 
            this.CognomeLabel.AutoSize = true;
            this.CognomeLabel.Location = new System.Drawing.Point(12, 88);
            this.CognomeLabel.Name = "CognomeLabel";
            this.CognomeLabel.Size = new System.Drawing.Size(52, 13);
            this.CognomeLabel.TabIndex = 2;
            this.CognomeLabel.Text = "Cognome";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(12, 140);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(55, 13);
            this.UsernameLabel.TabIndex = 3;
            this.UsernameLabel.Text = "Username";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(12, 166);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.PasswordLabel.TabIndex = 4;
            this.PasswordLabel.Text = "Password";
            // 
            // NomeTextBox
            // 
            this.NomeTextBox.Location = new System.Drawing.Point(73, 59);
            this.NomeTextBox.Name = "NomeTextBox";
            this.NomeTextBox.Size = new System.Drawing.Size(228, 20);
            this.NomeTextBox.TabIndex = 5;
            // 
            // CognomeTextBox
            // 
            this.CognomeTextBox.Location = new System.Drawing.Point(73, 85);
            this.CognomeTextBox.Name = "CognomeTextBox";
            this.CognomeTextBox.Size = new System.Drawing.Size(228, 20);
            this.CognomeTextBox.TabIndex = 6;
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(73, 137);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(228, 20);
            this.UsernameTextBox.TabIndex = 7;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(73, 163);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(228, 20);
            this.PasswordTextBox.TabIndex = 8;
            // 
            // SigninButton
            // 
            this.SigninButton.Location = new System.Drawing.Point(17, 206);
            this.SigninButton.Name = "SigninButton";
            this.SigninButton.Size = new System.Drawing.Size(68, 23);
            this.SigninButton.TabIndex = 9;
            this.SigninButton.Text = "Sign-In";
            this.SigninButton.UseVisualStyleBackColor = true;
            this.SigninButton.Click += new System.EventHandler(this.SigninButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(226, 206);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Annulla";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Location = new System.Drawing.Point(73, 111);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(228, 20);
            this.EmailTextBox.TabIndex = 11;
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(12, 114);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(32, 13);
            this.EmailLabel.TabIndex = 12;
            this.EmailLabel.Text = "Email";
            // 
            // FormSignin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 241);
            this.ControlBox = false;
            this.Controls.Add(this.EmailLabel);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SigninButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.CognomeTextBox);
            this.Controls.Add(this.NomeTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.CognomeLabel);
            this.Controls.Add(this.NomeLabel);
            this.Controls.Add(this.lblSignin);
            this.Name = "FormSignin";
            this.Text = "Sign-In";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSignin;
        private System.Windows.Forms.Label NomeLabel;
        private System.Windows.Forms.Label CognomeLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox NomeTextBox;
        private System.Windows.Forms.TextBox CognomeTextBox;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button SigninButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Label EmailLabel;
    }
}