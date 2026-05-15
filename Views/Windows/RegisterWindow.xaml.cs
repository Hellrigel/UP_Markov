using System.Windows;
using UP_Markov.Services;

namespace UP_Markov.Views.Windows
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService authService =
            new AuthService();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            bool result = authService.Register(
                LoginBox.Text,
                PasswordBox.Password,
                EmailBox.Text,
                DisplayNameBox.Text);

            if (!result)
            {
                MessageBox.Show("Пользователь уже существует");
                return;
            }

            MessageBox.Show("Аккаунт создан");

            LoginWindow loginWindow =
                new LoginWindow();

            loginWindow.Show();

            Close();
        }

        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoginWindow loginWindow =
                new LoginWindow();

            loginWindow.Show();

            Close();
        }
    }
}