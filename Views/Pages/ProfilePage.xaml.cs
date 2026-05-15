using System.Windows.Controls;
using UP_Markov.Data;

namespace UP_Markov.Views.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();

            LoginText.Text =
                $"Логин: {CurrentUser.User.Login}";

            EmailText.Text =
                $"Email: {CurrentUser.User.Email}";

            RoleText.Text =
                $"Роль: {CurrentUser.User.Roles.Name}";
        }
    }
}