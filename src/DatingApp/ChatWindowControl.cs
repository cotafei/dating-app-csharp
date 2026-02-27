using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatingApp
{
    public partial class ChatWindowControl : UserControl
    {
        private int chatId;
        private int currentUserId;
        private int companionId;
        private string companionName;
        private Image companionPhoto;

        public event EventHandler BackToChatList;

        public ChatWindowControl(int chatId, int currentUserId)
        {
            InitializeComponent();
            this.chatId = chatId;
            this.currentUserId = currentUserId;

            LoadCompanionInfo();
            SetupTopPanel();

            btnSend.Click += BtnSend_Click;
            LoadMessages();
        }

        private void LoadCompanionInfo()
        {
            using var conn = new MySqlConnection(DbConfig.ConnectionString);
            conn.Open();

            // Получаем собеседника по чату
            string getUsersSql = "SELECT user1_id, user2_id FROM chats WHERE id = @chatId";
            using var cmd1 = new MySqlCommand(getUsersSql, conn);
            cmd1.Parameters.AddWithValue("@chatId", chatId);

            using var reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                int user1 = reader1.GetInt32("user1_id");
                int user2 = reader1.GetInt32("user2_id");
                companionId = (user1 == currentUserId) ? user2 : user1;
            }
            reader1.Close();

            // Загружаем данные пользователя
            string getUserSql = "SELECT name, photo FROM users WHERE id = @id";
            using var cmd2 = new MySqlCommand(getUserSql, conn);
            cmd2.Parameters.AddWithValue("@id", companionId);

            using var reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                companionName = reader2.GetString("name");
                if (!reader2.IsDBNull(reader2.GetOrdinal("photo")))
                {
                    var photoBytes = (byte[])reader2["photo"];
                    using var ms = new MemoryStream(photoBytes);
                    companionPhoto = Image.FromStream(ms);
                }
                else
                {
                    companionPhoto = null;
                }
            }
        }

        private void SetupTopPanel()
        {
            pictureBoxAvatar = new PictureBox
            {
                Width = 40,
                Height = 40,
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = companionPhoto // устанавливаем фото из базы
            };

            labelCompanionName = new Label
            {
                Text = companionName,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(60, 18),
                AutoSize = true
            };

            btnBack = new Button
            {
                Text = "← Назад",
                Location = new Point(Width - 90, 15),
                Width = 80,
                Height = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnBack.Click += (s, e) => BackToChatList?.Invoke(this, EventArgs.Empty);

            topPanel.Controls.Add(pictureBoxAvatar);
            topPanel.Controls.Add(labelCompanionName);
            topPanel.Controls.Add(btnBack);
            Controls.Add(topPanel);
            topPanel.BringToFront();
        }

        private void LoadMessages()
        {
            flowLayoutMessages.Controls.Clear();

            using var conn = new MySqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = @"
                SELECT sender_id, message_text, timestamp 
                FROM messages 
                WHERE chat_id = @chatId 
                ORDER BY timestamp ASC";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@chatId", chatId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int senderId = reader.GetInt32("sender_id");
                string messageText = reader.GetString("message_text");
                DateTime timestamp = reader.GetDateTime("timestamp");

                var messageControl = CreateMessageControl(senderId == currentUserId, messageText, timestamp);
                flowLayoutMessages.Controls.Add(messageControl);
            }

            if (flowLayoutMessages.Controls.Count > 0)
                flowLayoutMessages.ScrollControlIntoView(flowLayoutMessages.Controls[flowLayoutMessages.Controls.Count - 1]);
        }

        private Control CreateMessageControl(bool isCurrentUser, string text, DateTime timestamp)
        {
            var panel = new Panel
            {
                Width = flowLayoutMessages.Width - 25,
                Height = 50,
                Margin = new Padding(5)
            };

            var labelText = new Label
            {
                Text = text,
                AutoSize = false,
                MaximumSize = new Size(panel.Width - 20, 0),
                Font = new Font("Segoe UI", 10),
                Size = new Size(panel.Width / 2 - 20, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = isCurrentUser ? Color.LightGreen : Color.LightGray,
                Padding = new Padding(8),
                BorderStyle = BorderStyle.FixedSingle
            };

            var labelTime = new Label
            {
                Text = timestamp.ToString("HH:mm"),
                Font = new Font("Segoe UI", 7),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            if (isCurrentUser)
            {
                labelText.Location = new Point(panel.Width - labelText.Width - 10, 5);
                labelText.TextAlign = ContentAlignment.MiddleRight;
                labelTime.Location = new Point(labelText.Left - 45, 30);
            }
            else
            {
                labelText.Location = new Point(10, 5);
                labelTime.Location = new Point(labelText.Right + 5, 30);
            }

            panel.Controls.Add(labelText);
            panel.Controls.Add(labelTime);
            return panel;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            string newMessage = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(newMessage))
                return;

            using var conn = new MySqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string insertSql = @"
                INSERT INTO messages (chat_id, sender_id, message_text, timestamp)
                VALUES (@chatId, @senderId, @messageText, @timestamp)";

            using var cmd = new MySqlCommand(insertSql, conn);
            cmd.Parameters.AddWithValue("@chatId", chatId);
            cmd.Parameters.AddWithValue("@senderId", currentUserId);
            cmd.Parameters.AddWithValue("@messageText", newMessage);
            cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);

            cmd.ExecuteNonQuery();

            txtMessage.Clear();
            LoadMessages();
        }

        private void flowLayoutMessages_Paint(object sender, PaintEventArgs e)
        {
            // Не используется
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            // Можно добавить логику активации кнопки отправки
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            BtnSend_Click(sender, e);
        }
    }
}
