using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatingApp
{
    public partial class ProfilesControl : UserControl
    {
        private int userId;

        public ProfilesControl(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadProfiles();
        }

        private void LoadProfiles()
        {
            flowLayoutProfiles.Controls.Clear();

            using var conn = new MySqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = "SELECT id, name, age, interests, photo FROM users WHERE id != @userId LIMIT 8";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32("id");
                string name = reader.GetString("name");
                int age = reader.GetInt32("age");
                string interests = reader.IsDBNull(reader.GetOrdinal("interests")) ? "" : reader.GetString("interests");

                Image avatarImage = null;
                if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                {
                    try
                    {
                        byte[] photoBytes = (byte[])reader["photo"];
                        if (photoBytes != null && photoBytes.Length > 0)
                        {
                            using var ms = new MemoryStream(photoBytes);
                            avatarImage = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        avatarImage = null; // если ошибка, просто не показываем фото
                    }
                }

                var card = CreateProfileCard(id, name, age, interests, avatarImage);
                flowLayoutProfiles.Controls.Add(card);
            }
        }


        private Panel CreateProfileCard(int id, string name, int age, string interests, Image avatarImage)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 280,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox avatar = new PictureBox
            {
                Width = 80,
                Height = 80,
                Location = new Point(60, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = avatarImage
            };

            Label nameLabel = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(10, 100),
                AutoSize = true
            };

            Label ageLabel = new Label
            {
                Text = $"Возраст: {age}",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 130),
                AutoSize = true
            };

            Label interestsLabel = new Label
            {
                Text = $"Интересы: {interests}",
                Font = new Font("Segoe UI", 8F),
                Location = new Point(10, 160),
                Size = new Size(180, 60),
                AutoEllipsis = true
            };

            Button likeButton = new Button
            {
                Text = "♥ Лайк",
                BackColor = Color.LightPink,
                Location = new Point(50, 230),
                Width = 100
            };
            likeButton.Click += (s, e) => HandleLikeClick(id, name);

            card.Controls.Add(avatar);
            card.Controls.Add(nameLabel);
            card.Controls.Add(ageLabel);
            card.Controls.Add(interestsLabel);
            card.Controls.Add(likeButton);

            return card;
        }

        private void HandleLikeClick(int likedId, string likedName)
        {
            try
            {
                using var conn = new MySqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var transaction = conn.BeginTransaction();

                using (var checkExisting = new MySqlCommand("SELECT 1 FROM likes WHERE liker_id = @liker AND liked_id = @liked", conn, transaction))
                {
                    checkExisting.Parameters.AddWithValue("@liker", userId);
                    checkExisting.Parameters.AddWithValue("@liked", likedId);

                    if (checkExisting.ExecuteScalar() != null)
                    {
                        MessageBox.Show("Вы уже лайкали этого пользователя.");
                        return;
                    }
                }

                using (var insertCmd = new MySqlCommand("INSERT INTO likes (liker_id, liked_id) VALUES (@liker, @liked)", conn, transaction))
                {
                    insertCmd.Parameters.AddWithValue("@liker", userId);
                    insertCmd.Parameters.AddWithValue("@liked", likedId);
                    insertCmd.ExecuteNonQuery();
                }

                using (var matchCheck = new MySqlCommand("SELECT 1 FROM likes WHERE liker_id = @liked AND liked_id = @liker", conn, transaction))
                {
                    matchCheck.Parameters.AddWithValue("@liked", likedId);
                    matchCheck.Parameters.AddWithValue("@liker", userId);

                    if (matchCheck.ExecuteScalar() != null)
                    {
                        using var chatCmd = new MySqlCommand("INSERT INTO chats (user1_id, user2_id) VALUES (@u1, @u2)", conn, transaction);
                        chatCmd.Parameters.AddWithValue("@u1", userId);
                        chatCmd.Parameters.AddWithValue("@u2", likedId);
                        chatCmd.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show($"💬 У вас взаимный лайк с {likedName}! Чат создан.");
                    }
                    else
                    {
                        transaction.Commit();
                        MessageBox.Show($"Вы лайкнули {likedName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обработке лайка: " + ex.Message);
            }
        }

        private void flowLayoutProfiles_Paint(object sender, PaintEventArgs e)
        {
            // Не используется
        }
    }
}
