using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UP_Markov.Data;

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

            LoadFrozenBooks();
        }

        private void LoadBooks()
        {
            BooksPanel.Children.Clear();

            var books = db.Books
                .Where(x =>
                    x.AuthorId ==
                    Data.CurrentUser.User.Id
                    &&

                    !x.IsFrozen)
                .OrderByDescending(x => x.Id)
                .ToList();

            if (!books.Any())
            {
                TextBlock empty =
                    new TextBlock()
                    {
                        Text =
                            "У вас пока нет книг",

                        FontSize = 20,

                        Foreground =
                            Brushes.Gray
                    };

                BooksPanel.Children.Add(empty);

                return;
            }

            foreach (var book in books)
            {
                Border border = new Border
                {
                    BorderBrush =
                        Brushes.Gray,

                    BorderThickness =
                        new Thickness(1),

                    CornerRadius =
                        new CornerRadius(10),

                    Padding =
                        new Thickness(15),

                    Margin =
                        new Thickness(0, 0, 0, 15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock title =
                    new TextBlock()
                    {
                        Text = book.Title,

                        FontSize = 22,

                        FontWeight =
                            FontWeights.Bold,

                        Foreground =
                            Brushes.White
                    };

                TextBlock description =
                    new TextBlock()
                    {
                        Text =
                            book.Description,

                        TextWrapping =
                            TextWrapping.Wrap,

                        Margin =
                            new Thickness(0, 10, 0, 10),

                        Foreground =
                            Brushes.White
                    };

                Button delete =
                    new Button()
                    {
                        Content = "Удалить",

                        Tag = book,

                        Height = 35
                    };

                delete.Click +=
                    DeleteBook_Click;

                panel.Children.Add(title);

                panel.Children.Add(description);

                panel.Children.Add(delete);

                border.Child = panel;

                BooksPanel.Children.Add(border);
            }
        }

        private void AddBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                TitleBox.Text))
            {
                MessageBox.Show(
                    "Введите название книги");

                return;
            }

            Books book =
     new Books()
     {
         Title =
             TitleBox.Text,

         Description =
             DescriptionBox.Text,

         Content =
             ContentBox.Text,

         CoverPath =
             CoverBox.Text,

         AuthorId =
             Data.CurrentUser
             .User.Id,

         IsFrozen = false,

         CreatedAt = DateTime.Now
     };


            db.Books.Add(book);

            db.SaveChanges();

            TitleBox.Clear();

            DescriptionBox.Clear();

            ContentBox.Clear();

            CoverBox.Clear();

            LoadBooks();

            MessageBox.Show(
                "Книга добавлена");
        }

        private void DeleteBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            Books book =
                (Books)((Button)sender).Tag;

            var result =
                MessageBox.Show(
                    "Удалить книгу?",
                    "Подтверждение",
                    MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            db.Books.Remove(book);

            db.SaveChanges();

            LoadBooks();

            MessageBox.Show(
                "Книга удалена");
        }

        private void LoadFrozenBooks()
        {
            FrozenBooksPanel.Children.Clear();

            var books = db.Books
                .Where(x =>
                    x.AuthorId ==
                    Data.CurrentUser.User.Id
                    &&

                    x.IsFrozen)
                .ToList();

            if (!books.Any())
            {
                return;
            }

            foreach (var book in books)
            {
                Border border =
                    new Border
                    {
                        BorderBrush =
                            Brushes.Red,

                        BorderThickness =
                            new Thickness(1),

                        CornerRadius =
                            new CornerRadius(10),

                        Padding =
                            new Thickness(15),

                        Margin =
                            new Thickness(0, 0, 0, 15)
                    };

                StackPanel panel =
                    new StackPanel();

                TextBlock title =
                    new TextBlock()
                    {
                        Text = book.Title,

                        FontSize = 20,

                        FontWeight =
                            FontWeights.Bold,

                        Foreground =
                            Brushes.White
                    };

                TextBlock reason =
                    new TextBlock()
                    {
                        Text =
                            $"Причина: " +
                            $"{book.FreezeReason}",

                        Foreground =
                            Brushes.OrangeRed,

                        Margin =
                            new Thickness(0, 10, 0, 10)
                    };

                Button request =
                    new Button()
                    {
                        Content =
                            "Подать заявку",

                        Tag = book,

                        Height = 35
                    };

                request.Click +=
                    UnfreezeBook_Click;

                panel.Children.Add(title);

                panel.Children.Add(reason);

                panel.Children.Add(request);

                border.Child = panel;

                FrozenBooksPanel.Children.Add(border);
            }
        }

        private void UnfreezeBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            Books book =
                (Books)((Button)sender).Tag;

            bool exists =
                db.UnfreezeRequests.Any(x =>
                    x.BookId == book.Id);

            if (exists)
            {
                MessageBox.Show(
                    "Заявка уже отправлена");

                return;
            }

            UnfreezeRequests request =
                new UnfreezeRequests()
                {
                    BookId = book.Id,

                    Reason =
                        "Прошу разморозить книгу"
                };

            db.UnfreezeRequests.Add(
                request);

            db.SaveChanges();

            MessageBox.Show(
                "Заявка отправлена");
        }
    }
}