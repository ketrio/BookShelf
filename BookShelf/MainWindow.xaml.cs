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
using Microsoft.Win32;
using BookShelf.PluginSystem;

namespace BookShelf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public App AppCur { get; } = Application.Current as App;

        public MainWindow()
        {
            InitializeComponent();
            

            foreach (Type type in (Application.Current as App).plugins)
            {
                MenuItem item = new MenuItem();
                item.Header = (type.GetCustomAttributes(typeof(PluginInfo), false)[0] as PluginInfo).Name;
                
                item.Click += new RoutedEventHandler((obj, e) =>
                    ((IPlugin)Activator.CreateInstance(type, Application.Current)).Impact());

                PluginMenu.Items.Add(item);
            }
        }

        // Delete Book
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Book> books = (Application.Current as App).LibraryData.Books;
            books.Remove(BookGrid.SelectedItem as Book);
            if (books.Count > 0)
            {
                BookGrid.SelectedIndex = 0;
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
            ObservableCollection<Publisher> publishers = (Application.Current as App).LibraryData.Publishers;
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
            ObservableCollection<Author> authors = (Application.Current as App).LibraryData.Authors;
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
            if (BookView != null)
                BookView.Visibility = Visibility.Visible;
        }

        private void PublisherGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (PublisherView != null)
                PublisherView.Visibility = Visibility.Visible;
        }

        private void AuthorGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (AuthorView != null)
                AuthorView.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    (Application.Current as App).LibraryData.watcher?.Dispose();
                    (Application.Current as App).LibraryData = LibraryData.Load(openFileDialog.FileName);
                    (Application.Current as App).LibraryData.watcher.Changed += (Application.Current as App).Watcher_Changed;
                    (Application.Current as App).LibraryData.watcher.EnableRaisingEvents = true;
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Corrupted data", "Serialization error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    (Application.Current as App).LibraryData.storagePath = saveFileDialog.FileName;
                    (Application.Current as App).LibraryData.Save();
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Corrupted data", "Serialization error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.Clear();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            var ordered = from book in AppCur.LibraryData.Books
                                       orderby book.Title
                                       select book;
            AppCur.LibraryData.Books = new ObservableCollection<Book>(ordered);
            //AppCur.LibraryData.Books = new ObservableCollection<Book>(AppCur.LibraryData.Books.AsParallel().OrderBy(book => book.Title));
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            var ordered = from book in AppCur.LibraryData.Books
                          orderby book.Pages
                          select book;
            AppCur.LibraryData.Books = new ObservableCollection<Book>(ordered);
            //AppCur.LibraryData.Books = new ObservableCollection<Book>(AppCur.LibraryData.Books.AsParallel().OrderBy(book => book.Pages));
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            var list = new List<Book>();
            var grouped = from book in AppCur.LibraryData.Books
                          group book by book.Author into g
                          select g;
            foreach(var group in grouped)
            {
                foreach (var book in group)
                {
                    list.Add(book);
                }
            }
            AppCur.LibraryData.Books = new ObservableCollection<Book>(list);
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            var list = new List<Book>();
            var grouped = from book in AppCur.LibraryData.Books
                          group book by book.Publisher.Name into g
                          select g;
            foreach (var group in grouped)
            {
                foreach (var book in group)
                {
                    list.Add(book);
                }
            }
            AppCur.LibraryData.Books = new ObservableCollection<Book>(list);
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            var ordered = from publ in AppCur.LibraryData.Publishers
                          orderby publ.Name
                          select publ;
            AppCur.LibraryData.Publishers = new ObservableCollection<Publisher>(ordered);
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            var list = new List<Publisher>();
            var grouped = from publ in AppCur.LibraryData.Publishers
                          group publ by publ.Name into g
                          select g;
            foreach (var group in grouped)
            {
                foreach (var book in group)
                {
                    list.Add(book);
                }
            }
            AppCur.LibraryData.Publishers = new ObservableCollection<Publisher>(list);
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            var ordered = from author in AppCur.LibraryData.Authors
                          orderby author.Name
                          select author;
            AppCur.LibraryData.Authors = new ObservableCollection<Author>(ordered);
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            var ordered = from author in AppCur.LibraryData.Authors
                          orderby author.BirthDate
                          select author;
            AppCur.LibraryData.Authors = new ObservableCollection<Author>(ordered);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text.Length == 0)
            {
                BookGrid.ItemsSource = AppCur.LibraryData.Books;
            }
            else
            {
                BookGrid.ItemsSource = AppCur.LibraryData.Books
                    .Where(book => book.Title.ToLower().Contains(textBox.Text.ToLower()) || book.Tags.Any(tag => tag.ToLower().Contains(textBox.Text.ToLower())));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text.Length == 0)
            {
                PublisherGrid.ItemsSource = AppCur.LibraryData.Publishers;
            }
            else
            {
                PublisherGrid.ItemsSource = AppCur.LibraryData.Publishers.Where(publ => publ.Name.ToLower().Contains(textBox.Text.ToLower()));
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text.Length == 0)
            {
                PublisherGrid.ItemsSource = AppCur.LibraryData.Publishers;
            }
            else
            {
                PublisherGrid.ItemsSource = AppCur.LibraryData.Publishers.Where(publ => publ.Name.ToLower().Contains(textBox.Text.ToLower()));
            }
        }
    }
}