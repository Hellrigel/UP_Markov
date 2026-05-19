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
        }

        private void LoadComplaints()
        {
            ComplaintsPanel.Children.Clear();

            var complaints = db.Complaints
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

                StackPanel panel = new StackPanel();

                TextBlock text = new TextBlock
                {
                    Text =
                        $"Жалоба: {complaint.Reason}",

                    Foreground = Brushes.White,

                    FontSize = 18
                };

                panel.Children.Add(text);

                border.Child = panel;

                ComplaintsPanel.Children.Add(border);
            }
        }

        private void LoadUsers()
        {
            UsersPanel.Children.Clear();

            var users = db.Users.ToList();

            foreach (var user in users)
            {
                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 15)
                };

                StackPanel panel = new StackPanel();

                TextBlock text = new TextBlock
                {
                    Text =
                        $"{user.Login} | " +
                        $"{user.DisplayName}",

                    Foreground = Brushes.White,

                    FontSize = 18
                };

                panel.Children.Add(text);

                border.Child = panel;

                UsersPanel.Children.Add(border);
            }
        }
    }
}