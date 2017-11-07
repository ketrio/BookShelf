using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models;
using System.Collections.ObjectModel;

namespace BookShelf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).LibraryData;
        }

        // Delete Book
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Book> books = (Application.Current as App).LibraryData.books;
            books.Remove(BookGrid.SelectedItem as Book);
            if (books.Count > 0)
            {
                BookGrid.SelectedIndex = 0;
            }
            else
            {
                BookView.Visibility = Visibility.Collapsed;
            }
        }

        // Add Book
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Books();
            window.Owner = this;
            window.ShowDialog();
        }

        // Add Publisher
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new Publishers();
            window.Owner = this;
            window.ShowDialog();
        }

        // Add Author
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Authors window = new Authors();
            window.Owner = this;
            window.ShowDialog(); 
        }

        // Edit Book
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Book currentBook = BookGrid.SelectedItem as Book;
            Books books = new Books(currentBook);
            books.Owner = this;
            books.ShowDialog();
        }

        // Edit Publisher
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Publisher currentPublisher = PublisherGrid.SelectedItem as Publisher;
            Publishers publishers = new Publishers(currentPublisher);
            publishers.Owner = this;
            publishers.ShowDialog();
        }

        // Delete Publisher
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Publisher> publishers = (Application.Current as App).LibraryData.publishers;
            publishers.Remove(PublisherGrid.SelectedItem as Publisher);
            if (publishers.Count > 0)
            {
                PublisherGrid.SelectedIndex = 0;
            }
            else
            {
                PublisherView.Visibility = Visibility.Collapsed;
            }
        }


        // Edit Author
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Author currentAuthor = AuthorGrid.SelectedItem as Author;
            Authors authors = new Authors(currentAuthor);
            authors.Owner = this;
            authors.ShowDialog();
        }

        // Delete Book
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Author> authors = (Application.Current as App).LibraryData.authors;
            authors.Remove(AuthorGrid.SelectedItem as Author);
            if (authors.Count > 0)
            {
                AuthorGrid.SelectedIndex = 0;
            }
            else
            {
                AuthorView.Visibility = Visibility.Collapsed;
            }
        }

        private void BookGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            BookView.Visibility = Visibility.Visible;
        }

        private void PublisherGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            PublisherView.Visibility = Visibility.Visible;
        }

        private void AuthorGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            AuthorView.Visibility = Visibility.Visible;
        }
    }
}