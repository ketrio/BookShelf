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
        public ObservableCollection<Book> books { get; set; }
        public ObservableCollection<Author> authors { get; set; }
        public ObservableCollection<Publisher> publishers { get; set; }

        public int bookCount => books.Count;
        public int authorCount => authors.Count;
        public int publisherCount => publishers.Count;

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

            books.CollectionChanged += new NotifyCollectionChangedEventHandler(Books_CollectionChanged);
            authors.CollectionChanged += new NotifyCollectionChangedEventHandler(Authors_CollectionChanged);
            publishers.CollectionChanged += new NotifyCollectionChangedEventHandler(Publishers_CollectionChanged);
        }

        public LibraryData() {
            books = new ObservableCollection<Book>();
            authors = new ObservableCollection<Author>();
            publishers = new ObservableCollection<Publisher>();

            initHandlers();
        }
        
        public LibraryData(ObservableCollection<Book> b, ObservableCollection<Author> a, ObservableCollection<Publisher> p)
        {
            this.books = b;
            this.authors = a;
            this.publishers = p;

            initHandlers();
        }

        public LibraryData(LibraryData library) : this(library.books, library.authors, library.publishers) { }

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
            LibraryData = new LibraryData();
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
