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

            SortBox.SelectedIndex = 0;

            LoadBooks();
        }

        private void LoadGenres()
        {
            GenreBox.Items.Clear();

            GenreBox.Items.Add("Все жанры");

            foreach (var genre in db.Genres)
            {
                GenreBox.Items.Add(genre.Name);
            }

            GenreBox.SelectedIndex = 0;
        }

        private void LoadBooks()
        {
            books = db.Books
                .Where(x => !x.IsFrozen)
                .ToList();

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredBooks =
                books.AsQueryable();

            string search =
                SearchBox.Text?
                .ToLower()
                .Trim();

            // SEARCH

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredBooks =
                    filteredBooks.Where(x =>

                        x.Title.ToLower()
                            .Contains(search)

                        ||

                        x.Users.DisplayName
                            .ToLower()
                            .Contains(search));
            }

            // GENRE

            if (GenreBox.SelectedItem != null &&
                GenreBox.SelectedIndex != 0)
            {
                string genre =
                    GenreBox.SelectedItem
                    .ToString();

                filteredBooks =
                    filteredBooks.Where(x =>

                        x.Genres.Any(g =>
                            g.Name == genre));
            }

            // SORT

            switch (SortBox.SelectedIndex)
            {
                case 0:

                    filteredBooks =
                        filteredBooks
                        .OrderBy(x => x.Title);

                    break;

                case 1:

                    filteredBooks =
                        filteredBooks
                        .OrderByDescending(x =>

                            x.Reviews.Any(r =>
                                !r.IsFrozen)

                            ?

                            x.Reviews
                                .Where(r => !r.IsFrozen)
                                .Average(r => r.Rating)

                            :

                            0);

                    break;
            }

            ShowBooks(
                filteredBooks.ToList());
        }

        private void ShowBooks(
            List<Books> booksToShow)
        {
            BooksPanel.Children.Clear();

            if (!booksToShow.Any())
            {
                TextBlock empty =
                    new TextBlock()
                    {
                        Text =
                            "Книги не найдены",

                        FontSize = 24,

                        Foreground =
                            System.Windows.Media.Brushes.White,

                        Margin =
                            new System.Windows.Thickness(20)
                    };

                BooksPanel.Children.Add(empty);

                return;
            }

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
            if (IsLoaded)
            {
                ApplyFilters();
            }
        }

        private void GenreBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
            }
        }
    }
}