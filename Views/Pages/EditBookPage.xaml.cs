using System;
using System.Windows;
using System.Windows.Controls;
using UP_Markov.Data;
using UP_Markov.Views.Windows;

namespace UP_Markov.Views.Pages
{
    public partial class EditBookPage : Page
    {
        private readonly UP_MarkovDBEntities db =
            new UP_MarkovDBEntities();

        private Books editingBook;

        public EditBookPage(Books book)
        {
            InitializeComponent();

            editingBook = book;

            if (editingBook != null)
            {
                LoadBook();
            }
        }

        private void LoadBook()
        {
            TitleBox.Text =
                editingBook.Title;

            DescriptionBox.Text =
                editingBook.Description;

            ContentBox.Text =
                editingBook.Content;
        }

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                    TitleBox.Text))
            {
                MessageBox.Show(
                    "Введите название");

                return;
            }

           

            if (editingBook == null)
            {
                Books newBook =
                    new Books()
                    {
                        Title =
                            TitleBox.Text,

                        Description =
                            DescriptionBox.Text,

                        Content =
                            ContentBox.Text,

                        AuthorId =
                            Data.CurrentUser.User.Id,

                        CreatedAt =
                            DateTime.Now,

                        IsFrozen = false
                    };

                db.Books.Add(newBook);
            }

            

            else
            {
                editingBook.Title =
                    TitleBox.Text;

                editingBook.Description =
                    DescriptionBox.Text;

                editingBook.Content =
                    ContentBox.Text;
            }

            db.SaveChanges();

            MessageBox.Show(
                "Книга сохранена");

            MainWindow.Instance.MainFrame.Navigate(
                new AuthorPage());
        }
    }
}