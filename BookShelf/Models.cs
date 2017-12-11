using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;

namespace Models
{
    [Serializable]
    public class Book : INotifyPropertyChanged
    {
        public Author Author
        {
            get
            {
                return _author;
            }
            set
            {
                Author prev = Author;
                _author = value;

                if (value.Books != null && value.Books.IndexOf(this) == -1)
                {
                    value.Books.Add(this);
                    prev?.Books.Remove(this);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Author"));
            }
        }

        public Publisher Publisher
        {
            get
            {
                return _publisher;
            }
            set
            {
                Publisher prev = Publisher;
                _publisher = value;

                if (value.Books != null && value.Books.IndexOf(this) == -1)
                {
                    value.Books.Add(this);
                    prev?.Books.Remove(this);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Publisher"));
            }
        }

        public String[] Tags {
            get { return _tags; }
            set
            {
                _tags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tags"));
            }
        }

        public DateTime PublishDate {
            get { return _publishDate; }
            set
            {
                _publishDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("publishDate"));
            }
        }
        
        public String Title {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }

        public String ISBN {
            get
            {
                return _isbn;
            }
            set
            {
                _isbn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ISBN"));
            }
        }

        public int Pages {
            get
            {
                return _pagesCount;
            }
            set
            {
                _pagesCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pages"));
            }
        }

        int _pagesCount;
        String _isbn;
        String _title;
        DateTime _publishDate;
        String[] _tags;
        Author _author;
        Publisher _publisher;
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public Book()
        {
            PublishDate = DateTime.Now.Date;
        }

        public Book(String Title, String ISBN, Author Author, 
            Publisher Publisher, int pagesCount, String[] Tags, DateTime publishDate)
        {
            this.Title = Title;
            this.ISBN = ISBN;
            this.Author = Author;
            this.Publisher = Publisher;
            this.Pages = pagesCount;
            this.Tags = Tags;
            this.PublishDate = publishDate;
        }

        public override bool Equals(Object obj)
        {
            Book bookObj = obj as Book;

            if (bookObj == null)
                return false;
            else
            {
                return ISBN.Equals(bookObj.ISBN);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => Title;
    }

    [Serializable]
    public class Author : INotifyPropertyChanged, ISerializable
    {
        public String Name {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public BitmapImage Image {
            get { return _image; }
            set
            {
                _image = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
            }
        }
        public DateTime BirthDate {
            get { return _birthDate; }
            set
            {
                _birthDate = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BirthDate"));
            }
        }
        public List<Book> Books {
            get { return _books; }
            set
            {
                _books = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Books"));
            }
        }

        String _name;
        BitmapImage _image;
        DateTime _birthDate;
        List<Book> _books;
        
        static public readonly BitmapImage defaultImage = 
            new BitmapImage(new Uri(@"pack://application:,,,/Images/photo.png "));
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public Author()
        {
            Image = defaultImage;
            BirthDate = DateTime.Now.Date;
            _books = new List<Book>();
        }
        public Author(String name, DateTime birthDate, String path = null, params Book[] books)
        {
            this.Name = name;
            if (path == null || !File.Exists(path)) Image = defaultImage;
            else Image = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.BirthDate = birthDate;
            if (books != null)
                this.Books = new List<Book>(books);
        }

        public override bool Equals(object obj)
        {
            Author AuthorObj = obj as Author;
            if (AuthorObj == null)
                return false;
            else
                return (
                    Name.Equals(AuthorObj.Name) &&
                    BirthDate.Equals(AuthorObj.BirthDate)
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => Name;

        public static byte[] ImageToByte(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(Author));
            info.AddValue("Name", _name);
            info.AddValue("Image", ImageToByte(_image));
            info.AddValue("Birthdate", _birthDate);
            info.AddValue("Books", _books);
        }

        private Author(SerializationInfo info, StreamingContext ctx)
        {
            _image = ToImage(info.GetValue("Image", typeof(byte[])) as byte[]);
            _name = info.GetString("Name");
            _birthDate = info.GetDateTime("Birthdate");
            _books = info.GetValue("Books", typeof(List<Book>)) as List<Book>;
        }

    }

    [Serializable]
    public class Publisher : INotifyPropertyChanged
    {
        public String Name {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public String City {
            get { return _city; }
            set
            {
                _city = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("City"));
            }
        }
        public List<Book> Books {
            get { return _books; }
            set
            {
                _books = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Books"));       
            }
        }

        String _name;
        String _city;
        List<Book> _books;
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public Publisher() { }

        public Publisher(String name, String city, params Book[] books)
        {
            this.Name = name;
            this.City = city;
            if (books != null)
                this.Books = new List<Book>(books);
        }

        public override bool Equals(object obj)
        {
            Publisher PublisherObj = obj as Publisher;


            if (PublisherObj == null)
                return false;
            else
            {

                bool n = Name.Equals(PublisherObj.Name);
                bool c = City.Equals(PublisherObj.City);

                return (
                    Name.Equals(PublisherObj.Name) &&
                    City.Equals(PublisherObj.City)
                );
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => Name + " from " + City;
    }
}