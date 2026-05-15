using System.Windows;
using System.Windows.Controls;
using UP_Markov.Views.Pages;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Components
{
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();
        }

        private void CatalogButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new CatalogPage());
        }

        private void ProfileButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new ProfilePage());
        }
        private void ListsButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new ListsPage());
        }
    }
}