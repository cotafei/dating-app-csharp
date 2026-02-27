using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatingApp
{
    public partial class ChatControl : UserControl
    {
        private int userId;
        public event Action<int> ChatSelected;
        private FlowLayoutPanel flowLayoutChats;

        public ChatControl(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            flowLayoutChats = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.WhiteSmoke
            };

            panelContent.Controls.Add(flowLayoutChats);
            LoadChats();
        }

        private void LoadChats()
        {
            flowLayoutChats.Controls.Clear();

            try
            {
                using (var conn = new MySqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();

                    var chatList = new List<(int ChatId, int InterlocutorId)>();

                    // Получаем список чатов и собеседников
                    using (var cmd = new MySqlCommand(@"
                        SELECT c.id, 
                               CASE WHEN c.user1_id = @userId THEN c.user2_id ELSE c.user1_id END AS interlocutor_id
                        FROM chats c
                        WHERE c.user1_id = @userId OR c.user2_id = @userId", conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int chatId = reader.GetInt32(0);
                                int interlocutorId = reader.GetInt32(1);
                                chatList.Add((chatId, interlocutorId));
                            }
                        }
                    }

                    foreach (var (chatId, interlocutorId) in chatList)
                    {
                        string name = "";
                        Image avatarImage = null;

                        // Получаем имя и аватар (как BLOB)
                        using (var cmdUser = new MySqlCommand("SELECT name, photo FROM users WHERE id = @id", conn))
                        {
                            cmdUser.Parameters.AddWithValue("@id", interlocutorId);
                            using (var reader = cmdUser.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    name = reader.GetString("name");
                                    if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                                    {
                                        byte[] photoBytes = (byte[])reader["photo"];
                                        using (var ms = new MemoryStream(photoBytes))
                                        {
                                            avatarImage = Image.FromStream(ms);
                                        }
                                    }
                                }
                            }
                        }

                        string lastMessage = "(нет сообщений)";
                        DateTime? lastTime = null;

                        // Получаем последнее сообщение и время
                        using (var cmdMsg = new MySqlCommand(@"
                            SELECT message_text, timestamp 
                            FROM messages 
                            WHERE chat_id = @chatId 
                            ORDER BY timestamp DESC 
                            LIMIT 1", conn))
                        {
                            cmdMsg.Parameters.AddWithValue("@chatId", chatId);
                            using (var reader = cmdMsg.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    lastMessage = reader.GetString("message_text");
                                    lastTime = reader.GetDateTime("timestamp");
                                }
                            }
                        }

                        var chatPanel = CreateChatItem(chatId, name, lastMessage, lastTime, avatarImage);
                        flowLayoutChats.Controls.Add(chatPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке чатов:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateChatItem(int chatId, string name, string lastMessage, DateTime? time, Image avatar)
        {
            Panel panel = new Panel
            {
                Width = 640,
                Height = 70,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = chatId
            };

            PictureBox picture = new PictureBox
            {
                Width = 50,
                Height = 50,
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = avatar ?? GeneratePlaceholderAvatar()
            };

            Label lblName = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(70, 10),
                AutoSize = true
            };

            Label lblMessage = new Label
            {
                Text = lastMessage,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(70, 35),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            Label lblTime = new Label
            {
                Text = time.HasValue ? time.Value.ToString("HH:mm") : "",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                Location = new Point(570, 10),
                AutoSize = true
            };

            panel.Controls.Add(picture);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblMessage);
            panel.Controls.Add(lblTime);

            // Подписываемся на клик панели и вложенных контролов
            panel.Click += (s, e) => ChatSelected?.Invoke(chatId);
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.Click += (s, e) => ChatSelected?.Invoke(chatId);
            }

            return panel;
        }

        private Image GeneratePlaceholderAvatar()
        {
            Bitmap bmp = new Bitmap(50, 50);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                g.DrawString("?", new Font("Segoe UI", 20, FontStyle.Bold), Brushes.DarkGray, new PointF(10, 5));
            }
            return bmp;
        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
