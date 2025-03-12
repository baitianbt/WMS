namespace WMS.UI
{
    partial class LoginForm
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
            lblTitle = new Label();
            loginPanel = new Panel();
            formLayout = new TableLayoutPanel();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkRememberUsername = new CheckBox();
            btnLogin = new Button();
            lblVersion = new Label();
            loginPanel.SuspendLayout();
            formLayout.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Î¢ÈíÑÅºÚ", 16F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(576, 77);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Warehouse Management System";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // loginPanel
            // 
            loginPanel.Controls.Add(formLayout);
            loginPanel.Dock = DockStyle.Fill;
            loginPanel.Location = new Point(0, 77);
            loginPanel.Margin = new Padding(4, 5, 4, 5);
            loginPanel.Name = "loginPanel";
            loginPanel.Padding = new Padding(45, 46, 45, 46);
            loginPanel.Size = new Size(576, 294);
            loginPanel.TabIndex = 1;
            // 
            // formLayout
            // 
            formLayout.ColumnCount = 2;
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            formLayout.Controls.Add(lblUsername, 0, 0);
            formLayout.Controls.Add(txtUsername, 1, 0);
            formLayout.Controls.Add(lblPassword, 0, 1);
            formLayout.Controls.Add(txtPassword, 1, 1);
            formLayout.Controls.Add(chkRememberUsername, 1, 2);
            formLayout.Controls.Add(btnLogin, 1, 3);
            formLayout.Dock = DockStyle.Fill;
            formLayout.Location = new Point(45, 46);
            formLayout.Margin = new Padding(4, 5, 4, 5);
            formLayout.Name = "formLayout";
            formLayout.RowCount = 4;
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            formLayout.Size = new Size(486, 202);
            formLayout.TabIndex = 0;
            // 
            // lblUsername
            // 
            lblUsername.Dock = DockStyle.Fill;
            lblUsername.Location = new Point(4, 0);
            lblUsername.Margin = new Padding(4, 0, 4, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(137, 50);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Username:";
            lblUsername.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtUsername
            // 
            txtUsername.Dock = DockStyle.Fill;
            txtUsername.Location = new Point(149, 5);
            txtUsername.Margin = new Padding(4, 5, 4, 5);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(333, 27);
            txtUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.Dock = DockStyle.Fill;
            lblPassword.Location = new Point(4, 50);
            lblPassword.Margin = new Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(137, 50);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password:";
            lblPassword.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Location = new Point(149, 55);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(333, 27);
            txtPassword.TabIndex = 3;
            // 
            // chkRememberUsername
            // 
            chkRememberUsername.AutoSize = true;
            chkRememberUsername.Checked = true;
            chkRememberUsername.CheckState = CheckState.Checked;
            chkRememberUsername.Location = new Point(149, 105);
            chkRememberUsername.Margin = new Padding(4, 5, 4, 5);
            chkRememberUsername.Name = "chkRememberUsername";
            chkRememberUsername.Size = new Size(189, 24);
            chkRememberUsername.TabIndex = 4;
            chkRememberUsername.Text = "Remember Username";
            chkRememberUsername.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.Location = new Point(149, 155);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(333, 42);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += BtnLogin_Click;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Bottom;
            lblVersion.ForeColor = Color.Gray;
            lblVersion.Location = new Point(0, 371);
            lblVersion.Margin = new Padding(4, 0, 4, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(576, 46);
            lblVersion.TabIndex = 2;
            lblVersion.Text = "Version: 1.0.0";
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(576, 417);
            Controls.Add(loginPanel);
            Controls.Add(lblVersion);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            loginPanel.ResumeLayout(false);
            formLayout.ResumeLayout(false);
            formLayout.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.TableLayoutPanel formLayout;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkRememberUsername;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblVersion;
    }
} 