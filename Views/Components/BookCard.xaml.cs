using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Pages;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Components
{
    public partial class BookCard : UserControl
    {
        public Books Book { get; set; }

        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public BookCard(Books book)
        {
            InitializeComponent();

            Book = book;

            LoadData();
        }

        private void LoadData()
        {
            TitleText.Text = Book.Title;

            AuthorText.Text =
                $"Автор: {Book.Users.DisplayName}";

            double rating = 0;

            if (Book.Reviews.Any())
            {
                rating = Book.Reviews
                    .Average(x => x.Rating);
            }

            RatingText.Text =
                $"★ {rating:F1}";
        }

        private void OpenButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new BookPage(Book));
        }
    }
}