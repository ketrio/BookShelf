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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Media.Imaging;
using BookShelf.PluginSystem;

namespace BookShelf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    [Serializable]
    public class LibraryData : INotifyPropertyChanged
    {
        public ObservableCollection<Book> Books
        {
            get
            {
                return books;
            }
            set
            {
                books = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Books"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("bookCount"));
            }
        }
        public ObservableCollection<Author> Authors
        {
            get
            {
                return authors;
            }
            set
            {
                authors = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Authors"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("authorCount"));
            }
        }
        public ObservableCollection<Publisher> Publishers
        {
            get
            {
                return publishers;
            }
            set
            {
                publishers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Publishers"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("publisherCount"));
            }
        }

        ObservableCollection<Book> books;
        ObservableCollection<Author> authors;
        ObservableCollection<Publisher> publishers;

        public int bookCount => Books.Count;
        public int authorCount => Authors.Count;
        public int publisherCount => Publishers.Count;

        [NonSerialized]
        public String storagePath;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NonSerialized]
        public FileSystemWatcher watcher = new FileSystemWatcher();

        void initHandlers()
        {
            Action<object, NotifyCollectionChangedEventArgs> Books_CollectionChanged =
                (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs("bookCount"));

            Action<object, NotifyCollectionChangedEventArgs> Authors_CollectionChanged =
                (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs("authorCount"));

            Action<object, NotifyCollectionChangedEventArgs> Publishers_CollectionChanged =
                (sender, e) => PropertyChanged(this, new PropertyChangedEventArgs("publisherCount"));

            Books.CollectionChanged += new NotifyCollectionChangedEventHandler(Books_CollectionChanged);
            Authors.CollectionChanged += new NotifyCollectionChangedEventHandler(Authors_CollectionChanged);
            Publishers.CollectionChanged += new NotifyCollectionChangedEventHandler(Publishers_CollectionChanged);
        }

        public LibraryData() {
            books = new ObservableCollection<Book>();
            authors = new ObservableCollection<Author>();
            publishers = new ObservableCollection<Publisher>();

            //ObservableCollection<Author> authors = new ObservableCollection<Author>() {
            //    new Author("Mark Twain", new DateTime(1910, 4, 21)),
            //    new Author("Harper Lee", new DateTime(1926, 4, 28)),
            //    new Author("J.D. Salinger", new DateTime(1919, 1, 1)),
            //    new Author("Ray Bradbury", new DateTime(1920, 8, 22))
            //};

            //ObservableCollection<Publisher> publishers = new ObservableCollection<Publisher>() {
            //    new Publisher("Sun-Times Media Group", "Chicago"),
            //    new Publisher("J.B. Lippincott", "Philadelphia"),
            //    new Publisher("Little, Brown and Company", "Chicago"),
            //    new Publisher("Ballantine Books", "New York City")
            //};

            //ObservableCollection<Book> books = new ObservableCollection<Book>() {
            //    new Book("Roughing It", "9781539669357", authors[0], publishers[0], 608, new String[] { "mark twain", "travel", "humor" }, new DateTime(1872, 1, 1)),
            //    new Book("Too Kill a Mockingbird", "9788373015470", authors[1], publishers[1], 281, new String[] { "harper lee", "classics", "childhood" }, new DateTime(1960, 6, 11)),
            //    new Book("The Cathcer in the Rye", "9785699356492", authors[2], publishers[2], 214, new String[] { "school", "fiction", "young adult" }, new DateTime(1951, 6, 16)),
            //    new Book("Fahrenheit 451", "9788952708922", authors[3], publishers[3], 158, new String[] { "Dystopia", "fiction", "knowledge" }, new DateTime(1953, 10, 1))
            //};

            //Authors = authors;
            //Publishers = publishers;
            //Books = books;

            initHandlers();
        }
        
        public LibraryData(ObservableCollection<Book> b, ObservableCollection<Author> a, ObservableCollection<Publisher> p)
        {
            this.Books = b;
            this.Authors = a;
            this.Publishers = p;

            initHandlers();
        }

        public LibraryData(LibraryData library) : this(library.Books, library.Authors, library.Publishers) { }

        public LibraryData(String storagePath) : this()
        {
            if (!File.Exists(storagePath)) File.Create(storagePath).Close();
            this.storagePath = storagePath;
        }

        public static LibraryData Load(String path)
        {
            var formatter = new BinaryFormatter();
            using (Stream s = File.OpenRead(path))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Decompress))
                {
                    var libraryData = new LibraryData(formatter.Deserialize(ds) as LibraryData);

                    libraryData.watcher = new FileSystemWatcher(Path.GetDirectoryName(path));
                    libraryData.watcher.Filter = Path.GetFileName(path);
                    libraryData.storagePath = path;

                    return libraryData;
                }
            }
        }

        public void Save()
        {
            watcher.EnableRaisingEvents = false;
            var formatter = new BinaryFormatter();
            using (Stream s = File.OpenWrite(storagePath))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Compress))
                {
                    formatter.Serialize(ds, this);
                }
            }
            watcher.EnableRaisingEvents = true;
        }
    }

    public partial class App : Application, INotifyPropertyChanged
    {
        public LibraryData LibraryData {
            get { return _libraryData; }
            set {
                _libraryData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LibraryData"));
            }
        }
        LibraryData _libraryData;
        public List<Type> plugins = PluginLoader.Load(Path.GetFullPath("Plugins/"));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LibraryData = new LibraryData("BookShelf.dat");
            LibraryData.watcher.Changed += Watcher_Changed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var result = MessageBox.Show("File has been updated. Would you like to load update version?", "File update",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes)
            {
                LibraryData.watcher.Dispose();
                LibraryData = LibraryData.Load(LibraryData.storagePath);
                LibraryData.watcher.Changed += Watcher_Changed;
                LibraryData.watcher.EnableRaisingEvents = true;
            }
        }
    }
}
