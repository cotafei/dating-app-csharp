using System;
using System.Drawing;
using System.Windows.Forms;

namespace DatingApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelMainContent;
        private Panel panelBottomBar;
        private Button buttonProfiles;
        private Button buttonChat;
        private Button buttonAccount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMainContent = new Panel();
            panelBottomBar = new Panel();
            buttonAccount = new Button();
            buttonChat = new Button();
            buttonProfiles = new Button();
            panelBottomBar.SuspendLayout();
            SuspendLayout();
            // 
            // panelMainContent
            // 
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.Location = new Point(0, 0);
            panelMainContent.Name = "panelMainContent";
            panelMainContent.Size = new Size(721, 366);
            panelMainContent.TabIndex = 0;
            panelMainContent.Paint += panelMainContent_Paint;
            // 
            // panelBottomBar
            // 
            panelBottomBar.BackColor = Color.LightGray;
            panelBottomBar.Controls.Add(buttonAccount);
            panelBottomBar.Controls.Add(buttonChat);
            panelBottomBar.Controls.Add(buttonProfiles);
            panelBottomBar.Dock = DockStyle.Bottom;
            panelBottomBar.Location = new Point(0, 366);
            panelBottomBar.Name = "panelBottomBar";
            panelBottomBar.Size = new Size(721, 56);
            panelBottomBar.TabIndex = 1;
            // 
            // buttonAccount
            // 
            buttonAccount.Dock = DockStyle.Fill;
            buttonAccount.FlatStyle = FlatStyle.Flat;
            buttonAccount.Location = new Point(492, 0);
            buttonAccount.Name = "buttonAccount";
            buttonAccount.Size = new Size(229, 56);
            buttonAccount.TabIndex = 2;
            buttonAccount.Text = "Аккаунт";
            buttonAccount.UseVisualStyleBackColor = true;
            buttonAccount.Click += buttonAccount_Click;
            // 
            // buttonChat
            // 
            buttonChat.Dock = DockStyle.Left;
            buttonChat.FlatStyle = FlatStyle.Flat;
            buttonChat.Location = new Point(233, 0);
            buttonChat.Name = "buttonChat";
            buttonChat.Size = new Size(259, 56);
            buttonChat.TabIndex = 1;
            buttonChat.Text = "Чат";
            buttonChat.UseVisualStyleBackColor = true;
            buttonChat.Click += buttonChat_Click;
            // 
            // buttonProfiles
            // 
            buttonProfiles.Dock = DockStyle.Left;
            buttonProfiles.FlatStyle = FlatStyle.Flat;
            buttonProfiles.Location = new Point(0, 0);
            buttonProfiles.Name = "buttonProfiles";
            buttonProfiles.Size = new Size(233, 56);
            buttonProfiles.TabIndex = 0;
            buttonProfiles.Text = "Анкеты";
            buttonProfiles.UseVisualStyleBackColor = true;
            buttonProfiles.Click += buttonProfiles_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(721, 422);
            Controls.Add(panelMainContent);
            Controls.Add(panelBottomBar);
            Name = "MainForm";
            Text = "Dating App";
            Load += MainForm_Load;
            panelBottomBar.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
