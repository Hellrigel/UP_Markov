using System.Windows;
using UP_Markov.Views.Pages;

namespace UP_Markov.Views.Windows
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            MainFrame.Navigate(new CatalogPage());
        }
    }
}