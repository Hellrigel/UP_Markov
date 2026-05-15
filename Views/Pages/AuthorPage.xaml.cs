using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Pages
{
    public partial class AuthorPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public AuthorPage()
        {
            InitializeComponent();

            LoadBooks();
        }

        private void LoadBooks()
        {
            BooksPanel.Children.Clear();

            int currentUserId =
                Data.CurrentUser.User.Id;

            var books = db.Books
                .Where(x =>
                    x.AuthorId == currentUserId)
                .ToList();

            foreach (var book in books)
            {
                Border border = new Border
                {
                    BorderThickness =
                        new Thickness(1),

                    BorderBrush =
                        System.Windows.Media.Brushes.Gray,

                    CornerRadius =
                        new CornerRadius(10),

                    Margin =
                        new Thickness(0, 0, 0, 15),

                    Padding =
                        new Thickness(15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock title =
                    new TextBlock()
                    {
                        Text = book.Title,

                        FontSize = 24,

                        FontWeight =
                            FontWeights.Bold,

                        Foreground =
                            System.Windows.Media.Brushes.White
                    };

                TextBlock frozen =
                    new TextBlock()
                    {
                        Foreground =
                            System.Windows.Media.Brushes.Red,

                        Margin =
                            new Thickness(0, 5, 0, 5)
                    };

                if (book.IsFrozen)
                {
                    frozen.Text =
                        $"Заморожено: {book.FreezeReason}";
                }

                Button edit =
                    new Button()
                    {
                        Content =
                            "Редактировать",

                        Tag = book,

                        Margin =
                            new Thickness(0, 10, 0, 5),

                        Height = 35
                    };

                Button delete =
                    new Button()
                    {
                        Content =
                            "Удалить",

                        Tag = book,

                        Height = 35
                    };

                edit.Click += EditBook_Click;

                delete.Click += DeleteBook_Click;

                panel.Children.Add(title);

                panel.Children.Add(frozen);

                panel.Children.Add(edit);

                panel.Children.Add(delete);

                border.Child = panel;

                BooksPanel.Children.Add(border);
            }
        }

        private void AddBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(
                new EditBookPage(null));
        }

        private void EditBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            Books selectedBook =
                (Books)((Button)sender).Tag;

            MainWindow.Instance.MainFrame.Navigate(
                new EditBookPage(selectedBook));
        }

        private void DeleteBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            Books selectedBook =
                (Books)((Button)sender).Tag;

            MessageBoxResult result =
                MessageBox.Show(
                    "Удалить книгу?",
                    "Удаление",
                    MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
                return;

            db.Books.Remove(selectedBook);

            db.SaveChanges();

            LoadBooks();
        }
    }
}