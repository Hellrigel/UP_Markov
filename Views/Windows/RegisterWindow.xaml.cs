using System;
using System.Linq;
using System.Windows;
using UP_Markov.Data;

namespace UP_Markov.Views.Windows
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(PassBox.Password) ||
                string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните основные поля");
                return;
            }

            try
            {
                using (var db = new UP_MarkovDBEntities())
                {
                    if (db.Users.Any(u => u.Login == LoginBox.Text))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует");
                        return;
                    }

                    var newUser = new Users
                    {
                        Login = LoginBox.Text,
                        Email = EmailBox.Text,
                        DisplayName = string.IsNullOrWhiteSpace(DisplayNameBox.Text) ? LoginBox.Text : DisplayNameBox.Text,
                        PasswordHash = PassBox.Password,
                        RoleId = 1,
                        IsFrozen = false,
                        CreatedAt = DateTime.Now
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("Аккаунт успешно создан!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}