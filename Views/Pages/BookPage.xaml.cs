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

            LoadBook();

            LoadRating();

            LoadStatuses();

            LoadReviewRatingBox();

            LoadReviews();
        }

        
        
        

        private void LoadBook()
        {
            TitleText.Text =
                book.Title;

            AuthorText.Text =
                $"Автор: {book.Users.DisplayName}";

            DescriptionText.Text =
                book.Description;
        }

        
        
        

        private void LoadRating()
        {
            double rating = 0;

            if (book.Reviews.Any())
            {
                rating = book.Reviews
                    .Average(x => x.Rating);
            }

            RatingText.Text =
                $"★ {rating:F1} / 10";
        }

        
        
        

        private void LoadStatuses()
        {
            foreach (var status in db.ReadingStatuses)
            {
                StatusBox.Items.Add(status.Name);
            }

            StatusBox.SelectedIndex = 0;
        }

        
        
        

        private void LoadReviewRatingBox()
        {
            for (int i = 1; i <= 10; i++)
            {
                ReviewRatingBox.Items.Add(i);
            }

            ReviewRatingBox.SelectedIndex = 4;
        }

        
        
        

        private void LoadReviews()
        {
            ReviewsPanel.Children.Clear();

            foreach (var review in book.Reviews
                         .OrderByDescending(x => x.CreatedAt))
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(0, 0, 0, 15),
                    Padding = new Thickness(10)
                };

                StackPanel panel = new StackPanel();

                TextBlock userText = new TextBlock
                {
                    Text =
                        review.Users.DisplayName,

                    Foreground = Brushes.White,

                    FontWeight = FontWeights.Bold,

                    FontSize = 16
                };

                TextBlock ratingText = new TextBlock
                {
                    Text =
                        $"★ {review.Rating}",

                    Foreground = Brushes.Gold,

                    Margin = new Thickness(0, 5, 0, 5)
                };

                TextBlock reviewText = new TextBlock
                {
                    Text =
                        review.ReviewText,

                    Foreground = Brushes.White,

                    TextWrapping =
                        TextWrapping.Wrap
                };

                Button complaintButton = new Button
                {
                    Content =
                        "Пожаловаться",

                    Tag = review,

                    Height = 30,

                    Margin = new Thickness(0, 10, 0, 0)
                };

                complaintButton.Click +=
                    ComplaintReview_Click;

                panel.Children.Add(userText);

                panel.Children.Add(ratingText);

                panel.Children.Add(reviewText);

                panel.Children.Add(complaintButton);

                border.Child = panel;

                ReviewsPanel.Children.Add(border);
            }
        }

        
        
        

        private void ReadButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                book.Content);
        }

        
        
        

        private void AddReview_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                    ReviewBox.Text))
            {
                MessageBox.Show(
                    "Введите отзыв");

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
                        (int)ReviewRatingBox.SelectedItem,

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

       
       
        

        private void AddToList_Click(
            object sender,
            RoutedEventArgs e)
        {
            int statusId =
                StatusBox.SelectedIndex + 1;

            var existing =
                db.UserBookLists
                .FirstOrDefault(x =>

                    x.UserId ==
                    CurrentUser.User.Id

                    &&

                    x.BookId ==
                    book.Id);

            

            if (existing != null)
            {
                existing.StatusId =
                    statusId;
            }

            

            else
            {
                UserBookLists item =
                    new UserBookLists()
                    {
                        UserId =
                            CurrentUser.User.Id,

                        BookId =
                            book.Id,

                        StatusId =
                            statusId
                    };

                db.UserBookLists.Add(item);
            }

            db.SaveChanges();

            MessageBox.Show(
                "Список обновлен");
        }

        
       
        

        private void ComplaintBook_Click(
            object sender,
            RoutedEventArgs e)
        {
            Complaints complaint =
                new Complaints()
                {
                    UserId =
                        CurrentUser.User.Id,

                    BookId =
                        book.Id,

                    ComplaintTypeId = 1,

                    Reason =
                        "Жалоба на книгу",

                    IsResolved = false
                };

            db.Complaints.Add(
                complaint);

            db.SaveChanges();

            MessageBox.Show(
                "Жалоба отправлена");
        }

        
        
        

        private void ComplaintReview_Click(
            object sender,
            RoutedEventArgs e)
        {
            Reviews review =
                (Reviews)((Button)sender).Tag;

            Complaints complaint =
                new Complaints()
                {
                    UserId =
                        CurrentUser.User.Id,

                    ReviewId =
                        review.Id,

                    ComplaintTypeId = 2,

                    Reason =
                        "Жалоба на отзыв",

                    IsResolved = false
                };

            db.Complaints.Add(
                complaint);

            db.SaveChanges();

            MessageBox.Show(
                "Жалоба отправлена");
        }
    }
}