using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatingApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        // Хэширование SHA-256 для пароля
        private static string ComputeSha256Hash(string rawData)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }

        // Обработчик кнопки "Вход"
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            string hashedPassword = ComputeSha256Hash(password);

            try
            {
                using var conn = new MySqlConnection(DbConfig.ConnectionString);
                conn.Open();

                var cmd = new MySqlCommand("SELECT id FROM users WHERE login = @login AND password = @pass", conn);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@pass", hashedPassword);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int userId = reader.GetInt32("id");
                    this.Hide();
                    var mainForm = new MainForm(userId);
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        // Обработчик кнопки "Регистрация"
        private void buttonReg_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            string hashedPassword = ComputeSha256Hash(password);

            try
            {
                using var conn = new MySqlConnection(DbConfig.ConnectionString);
                conn.Open();

                var checkCmd = new MySqlCommand("SELECT id FROM users WHERE login = @login", conn);
                checkCmd.Parameters.AddWithValue("@login", login);
                object result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    var updateCmd = new MySqlCommand("UPDATE users SET password = @pass WHERE login = @login", conn);
                    updateCmd.Parameters.AddWithValue("@login", login);
                    updateCmd.Parameters.AddWithValue("@pass", hashedPassword);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Пароль обновлён для существующего пользователя.");
                }
                else
                {
                    // Добавление нового пользователя с минимальными полями
                    var insertCmd = new MySqlCommand(
                        "INSERT INTO users (login, password, name, age) VALUES (@login, @pass, @name, @age)", conn);
                    insertCmd.Parameters.AddWithValue("@login", login);
                    insertCmd.Parameters.AddWithValue("@pass", hashedPassword);
                    insertCmd.Parameters.AddWithValue("@name", "Без имени"); // можно заменить на ввод с текстбокса
                    insertCmd.Parameters.AddWithValue("@age", 18); // возраст по умолчанию

                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("Регистрация успешна!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации: " + ex.Message);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Инициализация при загрузке формы, если необходимо
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
