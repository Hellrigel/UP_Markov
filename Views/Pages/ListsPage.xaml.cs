using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Components;

namespace UP_Markov.Views.Pages
{
    public partial class ListsPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        private List<UserBookLists> userBooks =
            new List<UserBookLists>();

        public ListsPage()
        {
            InitializeComponent();

            StatusBox.SelectedIndex = 0;

            LoadBooks();
        }

        private void LoadBooks()
        {
            userBooks = db.UserBookLists
                .Where(x =>
                    x.UserId ==
                    CurrentUser.User.Id)
                .ToList();

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered =
                userBooks.AsQueryable();

            

            int statusId =
                StatusBox.SelectedIndex + 1;

            filtered = filtered.Where(x =>
                x.StatusId == statusId);

            

            string search =
                SearchBox.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(x =>

                    x.Books.Title
                        .ToLower()
                        .Contains(search)

                    ||

                    x.Books.Users.DisplayName
                        .ToLower()
                        .Contains(search));
            }

            ShowBooks(filtered.ToList());
        }

        private void ShowBooks(
            List<UserBookLists> books)
        {
            BooksPanel.Children.Clear();

            foreach (var item in books)
            {
                BooksPanel.Children.Add(
                    new BookCard(item.Books));
            }
        }

        private void StatusBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}