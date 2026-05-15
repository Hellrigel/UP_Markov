using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Components;

namespace UP_Markov.Views.Pages
{
    public partial class CatalogPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        private List<Books> books =
            new List<Books>();

        public CatalogPage()
        {
            InitializeComponent();

            LoadGenres();

            LoadBooks();
        }

        private void LoadGenres()
        {
            GenreBox.Items.Add("Все жанры");

            foreach (var genre in db.Genres)
            {
                GenreBox.Items.Add(genre.Name);
            }

            GenreBox.SelectedIndex = 0;
        }

        private void LoadBooks()
        {
            books = db.Books.ToList();

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredBooks = books.AsQueryable();

            

            string search =
                SearchBox.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredBooks = filteredBooks.Where(x =>
                    x.Title.ToLower().Contains(search) ||
                    x.Users.DisplayName.ToLower().Contains(search));
            }

            

            if (GenreBox.SelectedItem != null &&
                GenreBox.SelectedIndex != 0)
            {
                string genre =
                    GenreBox.SelectedItem.ToString();

                filteredBooks = filteredBooks.Where(x =>
                    x.Genres.Any(g =>
                        g.Name == genre));
            }

            

            if (SortBox.SelectedIndex == 0)
            {
                filteredBooks = filteredBooks
                    .OrderBy(x => x.Title);
            }

            else if (SortBox.SelectedIndex == 1)
            {
                filteredBooks = filteredBooks
                    .OrderByDescending(x =>
                        x.Reviews.Any()
                            ? x.Reviews.Average(r => r.Rating)
                            : 0);
            }

            ShowBooks(filteredBooks.ToList());
        }

        private void ShowBooks(
            List<Books> booksToShow)
        {
            BooksPanel.Children.Clear();

            foreach (var book in booksToShow)
            {
                BooksPanel.Children.Add(
                    new BookCard(book));
            }
        }

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void GenreBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}