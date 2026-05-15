using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Helpers;
using UP_Markov.Views.Pages;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Components
{
    public partial class BookCard : UserControl
    {
        public Books Book { get; set; }

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

            if (Book.Reviews.Any(x => !x.IsFrozen))
            {
                rating = Book.Reviews
                    .Where(x => !x.IsFrozen)
                    .Average(x => x.Rating);
            }

            RatingText.Text =
                $"★ {rating:F1}";

            CoverImage.Source =
                ImageHelper.LoadImage(
                    Book.CoverPath);
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