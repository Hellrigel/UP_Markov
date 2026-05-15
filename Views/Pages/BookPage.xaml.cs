using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class BookPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        private Books book;

        public BookPage(Books selectedBook)
        {
            InitializeComponent();

            book = db.Books
                .FirstOrDefault(x =>
                    x.Id == selectedBook.Id);

            if (book == null)
            {
                MessageBox.Show(
                    "Книга не найдена");

                return;
            }

            LoadBook();

            LoadRating();

            LoadRatingBox();

            LoadReviews();
        }

        private void LoadBook()
        {
            TitleText.Text =
                book.Title;

            AuthorText.Text =
                $"Автор: " +
                $"{book.Users.DisplayName}";

            DescriptionText.Text =
                book.Description;
        }

        private void LoadRating()
        {
            double rating = 0;

            var reviews =
                book.Reviews
                .Where(x => !x.IsFrozen);

            if (reviews.Any())
            {
                rating =
                    reviews
                    .Average(x => x.Rating);
            }

            RatingText.Text =
                $"★ {rating:F1} / 10";
        }

        private void LoadRatingBox()
        {
            RatingBox.Items.Clear();

            for (int i = 1; i <= 10; i++)
            {
                RatingBox.Items.Add(i);
            }

            RatingBox.SelectedIndex = 4;
        }

        private void LoadReviews()
        {
            ReviewsPanel.Children.Clear();

            var reviews =
                book.Reviews
                .Where(x => !x.IsFrozen)
                .OrderByDescending(x => x.Id)
                .ToList();

            if (!reviews.Any())
            {
                TextBlock empty =
                    new TextBlock()
                    {
                        Text =
                            "Отзывов пока нет",

                        FontSize = 18,

                        Foreground =
                            Brushes.Gray
                    };

                ReviewsPanel.Children.Add(empty);

                return;
            }

            foreach (var review in reviews)
            {
                Border border =
                    new Border()
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

                TextBlock user =
                    new TextBlock()
                    {
                        Text =
                            review.Users
                            .DisplayName,

                        FontSize = 18,

                        FontWeight =
                            FontWeights.Bold,

                        Foreground =
                            Brushes.White
                    };

                TextBlock text =
                    new TextBlock()
                    {
                        Text =
                            review.ReviewText,

                        TextWrapping =
                            TextWrapping.Wrap,

                        Margin =
                            new Thickness(0, 10, 0, 10),

                        Foreground =
                            Brushes.White
                    };

                TextBlock rating =
                    new TextBlock()
                    {
                        Text =
                            $"★ {review.Rating}",

                        FontSize = 16,

                        Foreground =
                            Brushes.Gold
                    };

                Button complain =
                    new Button()
                    {
                        Content =
                            "Пожаловаться",

                        Tag = review,

                        Height = 35,

                        Margin =
                            new Thickness(0, 10, 0, 0)
                    };

                complain.Click +=
                    ComplaintReview_Click;

                panel.Children.Add(user);

                panel.Children.Add(text);

                panel.Children.Add(rating);

                panel.Children.Add(complain);

                border.Child = panel;

                ReviewsPanel.Children.Add(border);
            }
        }

        private void AddReview_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                ReviewBox.Text))
            {
                MessageBox.Show(
                    "Введите текст отзыва");

                return;
            }

            bool alreadyExists =
                db.Reviews.Any(x =>

                    x.BookId == book.Id
                    &&

                    x.UserId ==
                    CurrentUser.User.Id);

            if (alreadyExists)
            {
                MessageBox.Show(
                    "Вы уже оставляли отзыв");

                return;
            }

            Reviews review =
                new Reviews()
                {
                    BookId = book.Id,

                    UserId =
                        CurrentUser.User.Id,

                    ReviewText =
                        ReviewBox.Text,

                    Rating =
                        (int)RatingBox.SelectedItem,

                    IsFrozen = false
                };

            db.Reviews.Add(review);

            db.SaveChanges();

            book = db.Books
                .First(x => x.Id == book.Id);

            ReviewBox.Clear();

            LoadRating();

            LoadReviews();

            MessageBox.Show(
                "Отзыв добавлен");
        }

        private void ComplaintReview_Click(
            object sender,
            RoutedEventArgs e)
        {
            Reviews review =
                (Reviews)((Button)sender).Tag;

            bool exists =
                db.Complaints.Any(x =>

                    x.UserId ==
                    CurrentUser.User.Id

                    &&

                    x.ReviewId ==
                    review.Id);

            if (exists)
            {
                MessageBox.Show(
                    "Жалоба уже отправлена");

                return;
            }

            Complaints complaint =
                new Complaints()
                {
                    UserId =
                        CurrentUser.User.Id,

                    ReviewId =
                        review.Id,

                    ComplaintTypeId = 2,

                    Reason =
                        "Жалоба на отзыв"
                };

            db.Complaints.Add(
                complaint);

            db.SaveChanges();

            MessageBox.Show(
                "Жалоба отправлена");
        }

        private void ComplaintBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            bool exists =
                db.Complaints.Any(x =>

                    x.UserId ==
                    CurrentUser.User.Id

                    &&

                    x.BookId ==
                    book.Id);

            if (exists)
            {
                MessageBox.Show(
                    "Жалоба уже отправлена");

                return;
            }

            Complaints complaint =
                new Complaints()
                {
                    UserId =
                        CurrentUser.User.Id,

                    BookId =
                        book.Id,

                    ComplaintTypeId = 1,

                    Reason =
                        "Жалоба на книгу"
                };

            db.Complaints.Add(
                complaint);

            db.SaveChanges();

            MessageBox.Show(
                "Жалоба отправлена");
        }

        private void ReadButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                book.Content);
        }
    }
}