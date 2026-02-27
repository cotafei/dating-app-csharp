using System;
using System.Windows.Forms;

namespace DatingApp
{
    public partial class MainForm : Form
    {
        private int userId;

        public MainForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // При загрузке показываем вкладку Анкеты
            SetMainContent(new ProfilesControl(userId));
        }

        public void SetMainContent(Control ctrl)
        {
            panelMainContent.Controls.Clear();
            ctrl.Dock = DockStyle.Fill;
            panelMainContent.Controls.Add(ctrl);
        }

        private void buttonProfiles_Click(object sender, EventArgs e)
        {
            SetMainContent(new ProfilesControl(userId));
        }

        private void buttonChat_Click(object sender, EventArgs e)
        {
            var chatControl = new ChatControl(userId);
            chatControl.ChatSelected += OpenChatWindow;
            SetMainContent(chatControl);
        }

        private void buttonAccount_Click(object sender, EventArgs e)
        {
            SetMainContent(new AccountControl(userId));
        }

        private void OpenChatWindow(int chatId)
        {
            var chatWindow = new ChatWindowControl(chatId, userId);
            SetMainContent(chatWindow);
        }

        private void panelMainContent_Paint(object sender, PaintEventArgs e)
        {
            // Можно оставить пустым
        }
    }
}
