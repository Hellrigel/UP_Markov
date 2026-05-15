using System.Linq;
using System.Windows.Controls;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class CatalogPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public CatalogPage()
        {
            InitializeComponent();

            LoadBooks();
        }

        private void LoadBooks()
        {
            BooksList.ItemsSource = db.Books.ToList();
        }
    }
}