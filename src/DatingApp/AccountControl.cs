using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 

namespace DatingApp
{
    public class AccountControl : UserControl
    {
        private int userId;
        private Label labelName;
        private TextBox textBoxName;
        private Label labelAge;
        private NumericUpDown numericAge;
        private Label labelAvatar;
        private PictureBox pictureBoxAvatar;
        private Button buttonUpload;
        private Label label1;
        private TextBox textBoxInteres;
        private Button buttonSave;

        public AccountControl(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            labelName = new Label();
            textBoxName = new TextBox();
            labelAge = new Label();
            numericAge = new NumericUpDown();
            labelAvatar = new Label();
            pictureBoxAvatar = new PictureBox();
            buttonUpload = new Button();
            buttonSave = new Button();
            label1 = new Label();
            textBoxInteres = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numericAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).BeginInit();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(20, 20);
            labelName.Name = "labelName";
            labelName.Size = new Size(34, 15);
            labelName.TabIndex = 2;
            labelName.Text = "Имя:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(100, 20);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(200, 23);
            textBoxName.TabIndex = 3;
            // 
            // labelAge
            // 
            labelAge.AutoSize = true;
            labelAge.Location = new Point(20, 60);
            labelAge.Name = "labelAge";
            labelAge.Size = new Size(53, 15);
            labelAge.TabIndex = 4;
            labelAge.Text = "Возраст:";
            // 
            // numericAge
            // 
            numericAge.Location = new Point(100, 60);
            numericAge.Minimum = new decimal(new int[] { 12, 0, 0, 0 });
            numericAge.Name = "numericAge";
            numericAge.Size = new Size(120, 23);
            numericAge.TabIndex = 5;
            numericAge.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // labelAvatar
            // 
            labelAvatar.AutoSize = true;
            labelAvatar.Location = new Point(20, 148);
            labelAvatar.Name = "labelAvatar";
            labelAvatar.Size = new Size(48, 15);
            labelAvatar.TabIndex = 6;
            labelAvatar.Text = "Аватар:";
            // 
            // pictureBoxAvatar
            // 
            pictureBoxAvatar.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxAvatar.Location = new Point(100, 148);
            pictureBoxAvatar.Name = "pictureBoxAvatar";
            pictureBoxAvatar.Size = new Size(100, 100);
            pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAvatar.TabIndex = 7;
            pictureBoxAvatar.TabStop = false;
            pictureBoxAvatar.Click += pictureBoxAvatar_Click;
            // 
            // buttonUpload
            // 
            buttonUpload.Location = new Point(206, 148);
            buttonUpload.Name = "buttonUpload";
            buttonUpload.Size = new Size(95, 23);
            buttonUpload.TabIndex = 8;
            buttonUpload.Text = "Загрузить фото";
            buttonUpload.Click += ButtonUpload_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(100, 254);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 9;
            buttonSave.Text = "Сохранить";
            buttonSave.Click += ButtonSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 105);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 1;
            label1.Text = "Интерес:";
            // 
            // textBoxInteres
            // 
            textBoxInteres.Location = new Point(100, 105);
            textBoxInteres.Name = "textBoxInteres";
            textBoxInteres.Size = new Size(200, 23);
            textBoxInteres.TabIndex = 0;
            // 
            // AccountControl
            // 
            BackColor = Color.WhiteSmoke;
            Controls.Add(textBoxInteres);
            Controls.Add(label1);
            Controls.Add(labelName);
            Controls.Add(textBoxName);
            Controls.Add(labelAge);
            Controls.Add(numericAge);
            Controls.Add(labelAvatar);
            Controls.Add(pictureBoxAvatar);
            Controls.Add(buttonUpload);
            Controls.Add(buttonSave);
            Name = "AccountControl";
            Size = new Size(488, 351);
            Load += AccountControl_Load;
            ((System.ComponentModel.ISupportInitialize)numericAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadUserData()
        {
            try
            {
                using var conn = new MySqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new MySqlCommand("SELECT name, age, interests, photo FROM users WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", userId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBoxName.Text = reader.GetString("name");

                    numericAge.Value = reader.IsDBNull(reader.GetOrdinal("age"))
                        ? numericAge.Minimum
                        : reader.GetInt32("age");

                    if (!reader.IsDBNull(reader.GetOrdinal("interests")))
                        textBoxInteres.Text = reader.GetString("interests");

                    if (!reader.IsDBNull(reader.GetOrdinal("photo")))
                    {
                        byte[] photoBytes = (byte[])reader["photo"];
                        if (photoBytes.Length > 0)
                        {
                            if (pictureBoxAvatar.Image != null)
                            {
                                pictureBoxAvatar.Image.Dispose();
                                pictureBoxAvatar.Image = null;
                            }
                            using var ms = new MemoryStream(photoBytes);
                            pictureBoxAvatar.Image = Image.FromStream(ms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных пользователя:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ButtonUpload_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите фото для загрузки"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(ofd.FileName);

                    using var conn = new MySqlConnection(DbConfig.ConnectionString);
                    conn.Open();

                    using var cmd = new MySqlCommand("UPDATE users SET photo = @photo WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@photo", imageBytes);
                    cmd.Parameters.AddWithValue("@id", userId);

                    cmd.ExecuteNonQuery();

                    if (pictureBoxAvatar.Image != null)
                    {
                        pictureBoxAvatar.Image.Dispose();
                        pictureBoxAvatar.Image = null;
                    }
                    using var ms = new MemoryStream(imageBytes);
                    pictureBoxAvatar.Image = Image.FromStream(ms);

                    MessageBox.Show("Фото успешно загружено и сохранено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке фото: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();
            int age = (int)numericAge.Value;
            string interests = textBoxInteres.Text.Trim();

            try
            {
                using var conn = new MySqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new MySqlCommand("UPDATE users SET name = @name, age = @age, interests = @interests WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@interests", interests);
                cmd.Parameters.AddWithValue("@id", userId);

                int affected = cmd.ExecuteNonQuery();
                MessageBox.Show(affected > 0 ? "Изменения сохранены" : "Пользователь не найден", "Результат");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void AccountControl_Load(object sender, EventArgs e)
        {
            // Можно использовать для дополнительной инициализации
        }

        private void pictureBoxAvatar_Click(object sender, EventArgs e)
        {

        }
    }
}
