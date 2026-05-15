using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class BookPage : Page
    {
        private readonly UP_MarkovDBEntities db = new UP_MarkovDBEntities();
        private Books book;

        public BookPage(Books selectedBook)
        {
            InitializeComponent();

            
            book = db.Books.FirstOrDefault(x => x.Id == selectedBook.Id);

            if (book == null)
            {
                MessageBox.Show("Книга не найдена");
                return;
            }

            LoadBookData();
            LoadStatuses();     
            LoadRatingOptions(); 
            LoadReviews();      
        }

        private void LoadBookData()
        {
            TitleText.Text = book.Title;
            AuthorText.Text = $"Автор: {book.Users.DisplayName}";
            DescriptionText.Text = book.Description;
            UpdateAverageRating();
        }

        private void UpdateAverageRating()
        {
            var reviews = book.Reviews.Where(x => !x.IsFrozen).ToList();
            double rating = reviews.Any() ? reviews.Average(x => x.Rating) : 0;
            RatingText.Text = $"★ {rating:F1} / 10";
        }

        private void LoadStatuses()
        {
            
            StatusBox.ItemsSource = db.ReadingStatuses.ToList();
            StatusBox.DisplayMemberPath = "Name";
            StatusBox.SelectedValuePath = "Id";

            
            var currentEntry = db.UserBookLists.FirstOrDefault(x =>
                x.UserId == CurrentUser.User.Id && x.BookId == book.Id);

            if (currentEntry != null)
                StatusBox.SelectedValue = currentEntry.StatusId;
        }

        private void LoadRatingOptions()
        {
            RatingBox.Items.Clear();
            for (int i = 1; i <= 10; i++) RatingBox.Items.Add(i);
            RatingBox.SelectedIndex = 9; 
        }

        
        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (StatusBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите статус из списка");
                return;
            }

            int statusId = (int)StatusBox.SelectedValue;
            var entry = db.UserBookLists.FirstOrDefault(x =>
                x.UserId == CurrentUser.User.Id && x.BookId == book.Id);

            if (entry != null)
            {
                entry.StatusId = statusId;
            }
            else
            {
                db.UserBookLists.Add(new UserBookLists
                {
                    UserId = CurrentUser.User.Id,
                    BookId = book.Id,
                    StatusId = statusId,
                    AddedAt = DateTime.Now
                });
            }

            db.SaveChanges();
            MessageBox.Show("Список обновлен!");
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReviewBox.Text))
            {
                MessageBox.Show("Напишите текст отзыва");
                return;
            }

            if (db.Reviews.Any(x => x.BookId == book.Id && x.UserId == CurrentUser.User.Id))
            {
                MessageBox.Show("Вы уже оставляли отзыв к этой книге");
                return;
            }

            db.Reviews.Add(new Reviews
            {
                BookId = book.Id,
                UserId = CurrentUser.User.Id,
                ReviewText = ReviewBox.Text,
                Rating = (int)RatingBox.SelectedItem,
                IsFrozen = false,
                CreatedAt = DateTime.Now
            });

            db.SaveChanges();
            ReviewBox.Clear();
            UpdateAverageRating();
            LoadReviews();
            MessageBox.Show("Отзыв опубликован!");
        }

        private void LoadReviews()
        {
            ReviewsPanel.Children.Clear();
            var reviews = book.Reviews.Where(x => !x.IsFrozen).OrderByDescending(x => x.Id).ToList();

            if (!reviews.Any())
            {
                ReviewsPanel.Children.Add(new TextBlock { Text = "Отзывов пока нет", Foreground = Brushes.Gray });
                return;
            }

            foreach (var rev in reviews)
            {
                var border = new Border { BorderBrush = Brushes.DimGray, BorderThickness = new Thickness(0, 0, 0, 1), Margin = new Thickness(0, 0, 0, 10), Padding = new Thickness(5) };
                var stack = new StackPanel();
                stack.Children.Add(new TextBlock { Text = $"{rev.Users.DisplayName} ★{rev.Rating}", FontWeight = FontWeights.Bold, Foreground = Brushes.Gold });
                stack.Children.Add(new TextBlock { Text = rev.ReviewText, Foreground = Brushes.White, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 5, 0, 5) });

                var btn = new Button { Content = "Пожаловаться", HorizontalAlignment = HorizontalAlignment.Right, Tag = rev };
                btn.Click += ComplaintReview_Click;
                stack.Children.Add(btn);

                border.Child = stack;
                ReviewsPanel.Children.Add(border);
            }
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e) => MessageBox.Show(book.Content ?? "Текст книги отсутствует");

        private void ComplaintBook_Click(object sender, RoutedEventArgs e)
        {
            if (db.Complaints.Any(x => x.UserId == CurrentUser.User.Id && x.BookId == book.Id))
            {
                MessageBox.Show("Жалоба уже на рассмотрении");
                return;
            }
            db.Complaints.Add(new Complaints { UserId = CurrentUser.User.Id, BookId = book.Id, ComplaintTypeId = 1, Reason = "Жалоба на книгу", CreatedAt = DateTime.Now });
            db.SaveChanges();
            MessageBox.Show("Жалоба отправлена");
        }

        private void ComplaintReview_Click(object sender, RoutedEventArgs e)
        {
            var rev = (Reviews)((Button)sender).Tag;
            if (db.Complaints.Any(x => x.UserId == CurrentUser.User.Id && x.ReviewId == rev.Id)) return;

            db.Complaints.Add(new Complaints { UserId = CurrentUser.User.Id, ReviewId = rev.Id, ComplaintTypeId = 2, Reason = "Жалоба на отзыв", CreatedAt = DateTime.Now });
            db.SaveChanges();
            MessageBox.Show("Жалоба на отзыв принята");
        }
    }
}