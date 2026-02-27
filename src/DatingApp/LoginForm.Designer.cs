using System.Drawing;
using System.Windows.Forms;

namespace DatingApp
{
    public partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox textBoxLogin;
        private TextBox textBoxPassword;
        private Button buttonLogin;
        private Button buttonReg;
        private Label labelLogin;
        private Label labelPassword;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBoxLogin = new TextBox();
            textBoxPassword = new TextBox();
            buttonLogin = new Button();
            buttonReg = new Button();
            labelLogin = new Label();
            labelPassword = new Label();
            SuspendLayout();
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(20, 40);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(200, 23);
            textBoxLogin.TabIndex = 1;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(20, 95);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(200, 23);
            textBoxPassword.TabIndex = 3;
            textBoxPassword.UseSystemPasswordChar = true;
            textBoxPassword.TextChanged += textBoxPassword_TextChanged;
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new Point(20, 135);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(200, 30);
            buttonLogin.TabIndex = 4;
            buttonLogin.Text = "Войти";
            buttonLogin.Click += buttonLogin_Click;
            // 
            // buttonReg
            // 
            buttonReg.Location = new Point(20, 180);
            buttonReg.Name = "buttonReg";
            buttonReg.Size = new Size(200, 31);
            buttonReg.TabIndex = 5;
            buttonReg.Text = "Зарегаться или сменить пороль";
            buttonReg.Click += buttonReg_Click;
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(20, 20);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(44, 15);
            labelLogin.TabIndex = 6;
            labelLogin.Text = "Логин:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(20, 75);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(52, 15);
            labelPassword.TabIndex = 7;
            labelPassword.Text = "Пароль:";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(250, 223);
            Controls.Add(buttonReg);
            Controls.Add(labelLogin);
            Controls.Add(textBoxLogin);
            Controls.Add(labelPassword);
            Controls.Add(textBoxPassword);
            Controls.Add(buttonLogin);
            Name = "LoginForm";
            Text = "Вход в DatingApp";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
