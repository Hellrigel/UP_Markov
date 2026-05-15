using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class AdminPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public AdminPage()
        {
            InitializeComponent();

            LoadComplaints();

            LoadUsers();

            LoadFrozenBooks();

            LoadAuthorRequests();

            LoadUnfreezeRequests();
        }

        

        private void LoadComplaints()
        {
            ComplaintsPanel.Children.Clear();

            var complaints = db.Complaints
                .Where(x => !x.IsResolved)
                .ToList();

            foreach (var complaint in complaints)
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock text =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,
                        TextWrapping = TextWrapping.Wrap
                    };

                text.Text =
                    $"Жалоба от: " +
                    $"{complaint.Users.Login}\n" +

                    $"Причина: " +
                    $"{complaint.Reason}";

                Button approve =
                    new Button()
                    {
                        Content = "Принять",
                        Tag = complaint,
                        Height = 35,
                        Margin = new Thickness(0, 10, 0, 5)
                    };

                Button reject =
                    new Button()
                    {
                        Content = "Отклонить",
                        Tag = complaint,
                        Height = 35
                    };

                approve.Click += ApproveComplaint_Click;

                reject.Click += RejectComplaint_Click;

                panel.Children.Add(text);

                panel.Children.Add(approve);

                panel.Children.Add(reject);

                border.Child = panel;

                ComplaintsPanel.Children.Add(border);
            }
        }

        private void ApproveComplaint_Click(
            object sender,
            RoutedEventArgs e)
        {
            Complaints complaint =
                (Complaints)((Button)sender).Tag;

            complaint.IsResolved = true;

            

            if (complaint.BookId != null)
            {
                complaint.Books.IsFrozen = true;

                complaint.Books.FreezeReason =
                    complaint.Reason;
            }

            

            if (complaint.ReviewId != null)
            {
                complaint.Reviews.IsFrozen = true;
            }

            

            if (complaint.TargetUserId != null)
            {
                complaint.Users1.IsFrozen = true;

                complaint.Users1.FreezeReason =
                    complaint.Reason;
            }

            db.SaveChanges();

            LoadComplaints();

            LoadUsers();

            LoadFrozenBooks();

            MessageBox.Show(
                "Жалоба принята");
        }

        private void RejectComplaint_Click(
            object sender,
            RoutedEventArgs e)
        {
            Complaints complaint =
                (Complaints)((Button)sender).Tag;

            complaint.IsResolved = true;

            db.SaveChanges();

            LoadComplaints();

            MessageBox.Show(
                "Жалоба отклонена");
        }

        

        private void LoadUsers()
        {
            UsersPanel.Children.Clear();

            foreach (var user in db.Users)
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock text =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,
                        Text =
                            $"{user.Login} | " +
                            $"{user.Roles.Name}"
                    };

                Button freeze =
                    new Button()
                    {
                        Content =
                            user.IsFrozen
                                ? "Разморозить"
                                : "Заморозить",

                        Tag = user,

                        Height = 35,

                        Margin =
                            new Thickness(0, 10, 0, 0)
                    };

                freeze.Click += FreezeUser_Click;

                panel.Children.Add(text);

                panel.Children.Add(freeze);

                border.Child = panel;

                UsersPanel.Children.Add(border);
            }
        }

        private void FreezeUser_Click(
            object sender,
            RoutedEventArgs e)
        {
            Users user =
                (Users)((Button)sender).Tag;

            user.IsFrozen = !user.IsFrozen;

            if (user.IsFrozen)
            {
                user.FreezeReason =
                    "Аккаунт заморожен администратором";
            }

            else
            {
                user.FreezeReason = null;
            }

            db.SaveChanges();

            LoadUsers();

            MessageBox.Show(
                "Статус обновлен");
        }

        

        private void LoadFrozenBooks()
        {
            FrozenBooksPanel.Children.Clear();

            var books = db.Books
                .Where(x => x.IsFrozen)
                .ToList();

            foreach (var book in books)
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock text =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,

                        Text =
                            $"{book.Title}\n" +
                            $"Причина: {book.FreezeReason}"
                    };

                Button unfreeze =
                    new Button()
                    {
                        Content = "Разморозить",
                        Tag = book,
                        Height = 35,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                unfreeze.Click += UnfreezeBook_Click;

                panel.Children.Add(text);

                panel.Children.Add(unfreeze);

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

            book.IsFrozen = false;

            book.FreezeReason = null;

            db.SaveChanges();

            LoadFrozenBooks();

            MessageBox.Show(
                "Книга разморожена");
        }

        

        private void LoadAuthorRequests()
        {
            AuthorRequestsPanel.Children.Clear();

            var requests = db.AuthorRequests
                .Where(x => x.IsApproved == null)
                .ToList();

            foreach (var request in requests)
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(0, 0, 0, 15),
                    Padding = new Thickness(15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock text =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,

                        Text =
                            $"Заявка от: " +
                            $"{request.Users.Login}"
                    };

                Button approve =
                    new Button()
                    {
                        Content = "Принять",
                        Tag = request,
                        Height = 35,
                        Margin = new Thickness(0, 10, 0, 5)
                    };

                Button reject =
                    new Button()
                    {
                        Content = "Отклонить",
                        Tag = request,
                        Height = 35
                    };

                approve.Click += ApproveAuthor_Click;

                reject.Click += RejectAuthor_Click;

                panel.Children.Add(text);

                panel.Children.Add(approve);

                panel.Children.Add(reject);

                border.Child = panel;

                AuthorRequestsPanel.Children.Add(border);
            }
        }

        private void ApproveAuthor_Click(
            object sender,
            RoutedEventArgs e)
        {
            AuthorRequests request =
                (AuthorRequests)((Button)sender).Tag;

            request.IsApproved = true;

            request.Users.RoleId = 2;

            db.SaveChanges();

            LoadAuthorRequests();

            LoadUsers();

            MessageBox.Show(
                "Роль автора выдана");
        }

        private void RejectAuthor_Click(
            object sender,
            RoutedEventArgs e)
        {
            AuthorRequests request =
                (AuthorRequests)((Button)sender).Tag;

            request.IsApproved = false;

            db.SaveChanges();

            LoadAuthorRequests();

            MessageBox.Show(
                "Заявка отклонена");
        }

        

        private void LoadUnfreezeRequests()
        {
            UnfreezeRequestsPanel.Children.Clear();

            var requests = db.UnfreezeRequests
                .Where(x => x.IsApproved == null)
                .ToList();

            foreach (var request in requests)
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(0, 0, 0, 15),
                    Padding = new Thickness(15)
                };

                StackPanel panel =
                    new StackPanel();

                TextBlock text =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,

                        Text =
                            $"Пользователь: " +
                            $"{request.Users.Login}"
                    };

                Button approve =
                    new Button()
                    {
                        Content = "Разморозить",
                        Tag = request,
                        Height = 35,
                        Margin = new Thickness(0, 10, 0, 5)
                    };

                Button reject =
                    new Button()
                    {
                        Content = "Отклонить",
                        Tag = request,
                        Height = 35
                    };

                approve.Click += ApproveUnfreeze_Click;

                reject.Click += RejectUnfreeze_Click;

                panel.Children.Add(text);

                panel.Children.Add(approve);

                panel.Children.Add(reject);

                border.Child = panel;

                UnfreezeRequestsPanel.Children.Add(border);
            }
        }

        private void ApproveUnfreeze_Click(
            object sender,
            RoutedEventArgs e)
        {
            UnfreezeRequests request =
                (UnfreezeRequests)((Button)sender).Tag;

            request.IsApproved = true;

            request.Users.IsFrozen = false;

            request.Users.FreezeReason = null;

            db.SaveChanges();

            LoadUnfreezeRequests();

            LoadUsers();

            MessageBox.Show(
                "Пользователь разморожен");
        }

        private void RejectUnfreeze_Click(
            object sender,
            RoutedEventArgs e)
        {
            UnfreezeRequests request =
                (UnfreezeRequests)((Button)sender).Tag;

            request.IsApproved = false;

            db.SaveChanges();

            LoadUnfreezeRequests();

            MessageBox.Show(
                "Заявка отклонена");
        }
    }
}