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
                BookInfo.Opacity = 0;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Book book;
            Window window = new Books(out book);
            window.Owner = this;
            window.ShowDialog();
            (Application.Current as App).LibraryData.books.Add(book);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new Publishers();
            window.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Authors window = new Authors(Authors.Impact.New);
            window.Owner = this;
            window.ShowDialog();
            

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Book currentBook = BookGrid.SelectedItem as Book;
            Books books = new Books(currentBook);
            books.Owner = this;
            books.ShowDialog();
        }
    }
}