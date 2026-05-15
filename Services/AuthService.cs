using System.Linq;
using UP_Markov.Data;

namespace UP_Markov.Services
{
    public class AuthService
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        public Users Login(string login, string password)
        {
            return db.Users.FirstOrDefault(x =>
                x.Login == login &&
                x.PasswordHash == password);
        }

        public bool Register(
            string login,
            string password,
            string email,
            string displayName)
        {
            bool exists = db.Users.Any(x =>
                x.Login == login);

            if (exists)
                return false;

            Users user = new Users()
            {
                Login = login,
                PasswordHash = password,
                Email = email,
                DisplayName = displayName,
                RoleId = 1,
                IsFrozen = false
            };

            db.Users.Add(user);

            db.SaveChanges();

            return true;
        }
    }
}