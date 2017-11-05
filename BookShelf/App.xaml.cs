using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookShelf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public class LibraryData : INotifyPropertyChanged
    {
        public ObservableCollection<Book> books { get; set; }
        public ObservableCollection<Author> authors { get; set; }
        public ObservableCollection<Publisher> publishers { get; set; }

        public int bookCount => books.Count;
        public int authorCount => authors.Count;
        public int publisherCount => publishers.Count;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public LibraryData(ObservableCollection<Book> b, ObservableCollection<Author> a, ObservableCollection<Publisher> p)
        {
            this.books = b;
            this.authors = a;
            this.publishers = p;

            Action<object, NotifyCollectionChangedEventArgs> Books_CollectionChanged = 
                (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs("bookCount"));

            Action<object, NotifyCollectionChangedEventArgs> Authors_CollectionChanged =
                (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs("authorCount"));

            Action<object, NotifyCollectionChangedEventArgs> Publishers_CollectionChanged =
                (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs("publisherCount"));

            books.CollectionChanged += new NotifyCollectionChangedEventHandler(Books_CollectionChanged);
            authors.CollectionChanged += new NotifyCollectionChangedEventHandler(Authors_CollectionChanged);
            publishers.CollectionChanged += new NotifyCollectionChangedEventHandler(Publishers_CollectionChanged);
        }
    }

    public partial class App : Application
    {
        public LibraryData LibraryData { get; set; }

        public App() : base()
        {
            ObservableCollection<Author> authors = new ObservableCollection<Author> {
                new Author("Mark Twain", new DateTime(1910, 4, 21)),
                new Author("Harper Lee", new DateTime(1926, 4, 28)),
                new Author("J.D. Salinger", new DateTime(1919, 1, 1)),
                new Author("Ray Bradbury", new DateTime(1920, 8, 22))
            };
            ObservableCollection<Publisher> publishers = new ObservableCollection<Publisher> {
                new Publisher("Sun-Times Media Group", "Chicago"),
                new Publisher("J.B. Lippincott", "Philadelphia"),
                new Publisher("Little, Brown and Company", "Chicago"),
                new Publisher("Ballantine Books", "New York City")
            };
            ObservableCollection<Book> books = new ObservableCollection<Book> {
                new Book("Roughing It", "9781539669357", authors[0], publishers[0], 608, new String[] { "mark twain", "travel", "humor" }, new DateTime(1872, 1, 1)),
                new Book("Too Kill a Mockingbird", "9788373015470", authors[1], publishers[1], 281, new String[] { "harper lee", "classics", "childhood" }, new DateTime(1960, 6, 11)),
                new Book("The Cathcer in the Rye", "9785699356492", authors[2], publishers[2], 214, new String[] { "school", "fiction", "young adult" }, new DateTime(1951, 6, 16)),
                new Book("Fahrenheit 451", "9788952708922", authors[3], publishers[3], 158, new String[] { "Dystopia", "fiction", "knowledge" }, new DateTime(1953, 10, 1))
            };
            
            LibraryData = new LibraryData(books, authors, publishers);
        }
    }
}
