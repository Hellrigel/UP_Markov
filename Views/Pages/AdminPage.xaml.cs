using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UP_Markov.Data;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Pages
{
    public partial class AdminPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public AdminPage()
        {
            InitializeComponent();

            CheckAccess();

            LoadComplaints();

            LoadUsers();
        }

        

        private void CheckAccess()
        {
            if (CurrentUser.User.RoleId != 3)
            {
                MessageBox.Show(
                    "Нет доступа");

                MainWindow.Instance.MainFrame.Navigate(
                    new CatalogPage());
            }
        }

        

        private void LoadComplaints()
        {
            ComplaintsPanel.Children.Clear();

            var complaints = db.Complaints
                .Where(x => !x.IsResolved)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            foreach (var complaint in complaints)
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

                TextBlock info =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,
                        TextWrapping =
                            TextWrapping.Wrap
                    };

                string target = "";

                

                if (complaint.BookId != null)
                {
                    target =
                        $"Книга: {complaint.Books.Title}";
                }

               

                if (complaint.ReviewId != null)
                {
                    target =
                        $"Отзыв ID: {complaint.ReviewId}";
                }

                

                if (complaint.TargetUserId != null)
                {
                    target =
                        $"Пользователь: {complaint.Users1.Login}";
                }

                info.Text =
                    $"Жалоба от: {complaint.Users.Login}\n" +
                    $"{target}\n" +
                    $"Причина: {complaint.Reason}";

                Button freeze =
                    new Button()
                    {
                        Content = "Заморозить",
                        Tag = complaint,
                        Height = 35,
                        Margin =
                            new Thickness(0, 10, 0, 5)
                    };

                Button reject =
                    new Button()
                    {
                        Content = "Отклонить",
                        Tag = complaint,
                        Height = 35
                    };

                freeze.Click += Freeze_Click;

                reject.Click += Reject_Click;

                panel.Children.Add(info);

                panel.Children.Add(freeze);

                panel.Children.Add(reject);

                border.Child = panel;

                ComplaintsPanel.Children.Add(border);
            }
        }

        

        private void LoadUsers()
        {
            UsersPanel.Children.Clear();

            foreach (var user in db.Users.ToList())
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

                TextBlock info =
                    new TextBlock()
                    {
                        Foreground = Brushes.White,
                        Text =
                            $"{user.Login} | " +
                            $"{user.Email} | " +
                            $"{user.Roles.Name}"
                    };

                ComboBox roleBox =
                    new ComboBox()
                    {
                        Width = 150,
                        Margin =
                            new Thickness(0, 10, 0, 10),

                        Tag = user
                    };

                foreach (var role in db.Roles)
                {
                    roleBox.Items.Add(role.Name);
                }

                roleBox.SelectedItem =
                    user.Roles.Name;

                Button saveRole =
                    new Button()
                    {
                        Content =
                            "Сохранить роль",

                        Height = 35,

                        Tag = roleBox
                    };

                Button freeze =
                    new Button()
                    {
                        Content =
                            user.IsFrozen
                                ? "Разморозить"
                                : "Заморозить",

                        Height = 35,

                        Margin =
                            new Thickness(0, 10, 0, 0),

                        Tag = user
                    };

                saveRole.Click += SaveRole_Click;

                freeze.Click += FreezeUser_Click;

                panel.Children.Add(info);

                panel.Children.Add(roleBox);

                panel.Children.Add(saveRole);

                panel.Children.Add(freeze);

                border.Child = panel;

                UsersPanel.Children.Add(border);
            }
        }

        

        private void Freeze_Click(
            object sender,
            RoutedEventArgs e)
        {
            Complaints complaint =
                (Complaints)((Button)sender).Tag;

            

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

            complaint.IsResolved = true;

            db.SaveChanges();

            LoadComplaints();

            MessageBox.Show(
                "Заморозка выполнена");
        }

        

        private void Reject_Click(
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

       

        private void SaveRole_Click(
            object sender,
            RoutedEventArgs e)
        {
            ComboBox box =
                (ComboBox)((Button)sender).Tag;

            Users user =
                (Users)box.Tag;

            string roleName =
                box.SelectedItem.ToString();

            Roles role =
                db.Roles.First(x =>
                    x.Name == roleName);

            user.RoleId = role.Id;

            db.SaveChanges();

            MessageBox.Show(
                "Роль обновлена");
        }

        
        
        

        private void FreezeUser_Click(
            object sender,
            RoutedEventArgs e)
        {
            Users user =
                (Users)((Button)sender).Tag;

            user.IsFrozen =
                !user.IsFrozen;

            if (!user.IsFrozen)
            {
                user.FreezeReason = null;
            }

            else
            {
                user.FreezeReason =
                    "Заморожено администратором";
            }

            db.SaveChanges();

            LoadUsers();

            MessageBox.Show(
                "Статус обновлен");
        }
    }
}