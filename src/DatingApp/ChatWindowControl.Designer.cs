namespace DatingApp
{
    partial class ChatWindowControl
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flowLayoutMessages;
        private TextBox txtMessage;
        private Button btnSend;

        private Panel topPanel;
        private PictureBox pictureBoxAvatar;
        private Label labelCompanionName;
        private Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flowLayoutMessages = new FlowLayoutPanel();
            txtMessage = new TextBox();
            btnSend = new Button();
            topPanel = new Panel();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.Location = new Point(0, 0);
            topPanel.Size = new Size(this.Width, 57);
            topPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            topPanel.BackColor = Color.Transparent; // или другой цвет
                                                    // 
                                                    // flowLayoutMessages
                                                    // 
            flowLayoutMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutMessages.AutoScroll = true;
            flowLayoutMessages.Location = new Point(0, 57);
            flowLayoutMessages.Name = "flowLayoutMessages";
            flowLayoutMessages.Size = new Size(715, 290);
            flowLayoutMessages.TabIndex = 0;
            flowLayoutMessages.Paint += flowLayoutMessages_Paint;
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtMessage.Location = new Point(0, 365);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(603, 23);
            txtMessage.TabIndex = 1;
            txtMessage.TextChanged += txtMessage_TextChanged;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.Location = new Point(609, 358);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(103, 35);
            btnSend.TabIndex = 2;
            btnSend.Text = "Отправить";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click_1;
            // 
            // ChatWindowControl
            // 
            Controls.Add(topPanel);
            Controls.Add(flowLayoutMessages);
            Controls.Add(txtMessage);
            Controls.Add(btnSend);
            Name = "ChatWindowControl";
            Size = new Size(715, 400);
            ResumeLayout(false);
            PerformLayout();
        }

    }
}
