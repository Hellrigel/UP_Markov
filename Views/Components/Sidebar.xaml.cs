using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Pages;

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
            AdminButton.Visibility =
                Visibility.Collapsed;

            AuthorButton.Visibility =
                Visibility.Collapsed;

            if (CurrentUser.User.RoleId == 3)
            {
                AdminButton.Visibility =
                    Visibility.Visible;
            }

            if (CurrentUser.User.RoleId == 2)
            {
                AuthorButton.Visibility =
                    Visibility.Visible;
            }
        }

        private void Catalog_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new CatalogPage());
        }

        private void Lists_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new ListsPage());
        }

        private void Author_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new AuthorPage());
        }

        private void Admin_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new AdminPage());
        }

        private void Profile_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new ProfilePage());
        }
    }
}