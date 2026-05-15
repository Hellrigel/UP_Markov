using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class ProfilePage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public ProfilePage()
        {
            InitializeComponent();

            LoadProfile();

            CheckRequests();
        }

        private void LoadProfile()
        {
            LoginText.Text =
                $"Логин: {CurrentUser.User.Login}";

            EmailText.Text =
                $"Email: {CurrentUser.User.Email}";

            RoleText.Text =
                $"Роль: {CurrentUser.User.Roles.Name}";

            

            if (CurrentUser.User.IsFrozen)
            {
                FreezeText.Text =
                    $"Аккаунт заморожен: " +
                    $"{CurrentUser.User.FreezeReason}";
            }

            else
            {
                FreezeText.Visibility =
                    Visibility.Collapsed;
            }

            

            if (CurrentUser.User.RoleId != 1)
            {
                AuthorRequestButton.Visibility =
                    Visibility.Collapsed;
            }

           

            if (!CurrentUser.User.IsFrozen)
            {
                UnfreezeRequestButton.Visibility =
                    Visibility.Collapsed;
            }
        }

        private void CheckRequests()
        {
            bool authorRequestExists =
                db.AuthorRequests.Any(x =>
                    x.UserId ==
                    CurrentUser.User.Id);

            if (authorRequestExists)
            {
                AuthorRequestButton.IsEnabled =
                    false;

                AuthorRequestButton.Content =
                    "Заявка уже отправлена";
            }

            bool unfreezeExists =
                db.UnfreezeRequests.Any(x =>
                    x.UserId ==
                    CurrentUser.User.Id);

            if (unfreezeExists)
            {
                UnfreezeRequestButton.IsEnabled =
                    false;

                UnfreezeRequestButton.Content =
                    "Заявка уже отправлена";
            }
        }

        

        private void AuthorRequestButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AuthorRequests request =
                new AuthorRequests()
                {
                    UserId =
                        CurrentUser.User.Id
                };

            db.AuthorRequests.Add(request);

            db.SaveChanges();

            MessageBox.Show(
                "Заявка отправлена");

            CheckRequests();
        }

        

        private void UnfreezeRequestButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            UnfreezeRequests request =
                new UnfreezeRequests()
                {
                    UserId =
                        CurrentUser.User.Id,

                    Reason =
                        "Прошу разморозить аккаунт"
                };

            db.UnfreezeRequests.Add(request);

            db.SaveChanges();

            MessageBox.Show(
                "Заявка отправлена");

            CheckRequests();
        }
    }
}