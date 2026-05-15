using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class BookPage : Page
    {
        private readonly Books book;

        public BookPage(Books selectedBook)
        {
            InitializeComponent();

            book = selectedBook;

            LoadData();
        }

        private void LoadData()
        {
            TitleText.Text = book.Title;

            AuthorText.Text =
                $"Автор: {book.Users.DisplayName}";

            DescriptionText.Text =
                book.Description;
        }

        private void ReadButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(book.Content);
        }
    }
}