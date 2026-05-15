using System.Windows;
using UP_Markov.Data;
using UP_Markov.Services;

namespace UP_Markov.Views.Windows
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService authService =
            new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var user = authService.Login(
                LoginBox.Text,
                PasswordBox.Password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            if (user.IsFrozen)
            {
                MessageBox.Show("Аккаунт заморожен");
                return;
            }

            CurrentUser.User = user;

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

            Close();
        }

        private void RegisterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            RegisterWindow registerWindow =
                new RegisterWindow();

            registerWindow.Show();

            Close();
        }
    }
}