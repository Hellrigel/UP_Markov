using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Pages;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Components
{
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();

            CheckRoles();
        }

        private void CheckRoles()
        {
            

            if (CurrentUser.User.RoleId == 1)
            {
                AuthorButton.Visibility =
                    Visibility.Collapsed;
            }

            

            if (CurrentUser.User.RoleId != 3)
            {
                AdminButton.Visibility =
                    Visibility.Collapsed;
            }
        }

        private void CatalogButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new CatalogPage());
        }

        private void ListsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new ListsPage());
        }

        private void AuthorButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new AuthorPage());
        }

        private void AdminButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new AdminPage());
        }

        private void ProfileButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new ProfilePage());
        }
    }
}