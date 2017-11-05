using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Models
{
    public class Book : INotifyPropertyChanged
    {
        public Author Author
        {
            get
            {
                return _Author;
            }
            set
            {
                if (value.books != null && value.books.IndexOf(this) == -1)
                {
                    value.books.Add(this);
                }
                _Author = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Author"));
            }
        }

        public Publisher Publisher
        {
            get
            {
                return _Publisher;
            }
            set
            {
                if (value.books != null && value.books.IndexOf(this) == -1)
                {
                    value.books.Add(this);
                }
                _Publisher = value;

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

        public DateTime publishDate {
            get { return _publishDate; }
            set
            {
                _publishDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("publishDate"));
            }
        }
        
        public String Title { get; set; }
        public String ISBN { get; set; }
        public int pagesCount { get; set; }

        DateTime _publishDate;
        String[] _tags;
        Author _Author;
        Publisher _Publisher;

        public event PropertyChangedEventHandler PropertyChanged;

        public Book() { }

        public Book(String Title, String ISBN, Author Author, Publisher Publisher, int pagesCount, String[] Tags, DateTime publishDate)
        {
            this.Title = Title;
            this.ISBN = ISBN;
            this.Author = Author;
            this.Publisher = Publisher;
            this.pagesCount = pagesCount;
            this.Tags = Tags;
            this.publishDate = publishDate;
        }

        public override bool Equals(Object obj)
        {
            Book bookObj = obj as Book;
            if (bookObj == null)
                return false;
            else
                return (
                    Title.Equals(bookObj.Title) &&
                    ISBN.Equals(bookObj.ISBN) &&
                    Author.Equals(bookObj.Author) &&
                    Publisher.Equals(bookObj.Publisher) &&
                    pagesCount.Equals(bookObj.pagesCount) &&
                    Tags.Equals(bookObj.Tags) &&
                    publishDate.Equals(bookObj.publishDate)
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Title + " by " + Author.name + ".";
        }
    }

    public class Author
    {
        public String name { get; set; }
        public Image image { get; set; }
        public DateTime birthDate { get; set; }
        public List<Book> books { get; set; }
        
        static public BitmapImage defaultImage { get { return new BitmapImage(new Uri(@"C:\Users\Gudoque\source\repos\BookShelf\photo.png", UriKind.Absolute)); } }

        public Author(String name, DateTime birthDate, String path = null, params Book[] books)
        {
            this.name = name;
            this.image = File.Exists(path) ? new Bitmap(path) : null;
            this.birthDate = birthDate;
            if (books != null)
                this.books = new List<Book>(books);
        }

        public override bool Equals(object obj)
        {
            Author AuthorObj = obj as Author;
            if (AuthorObj == null)
                return false;
            else
                return (
                    name.Equals(AuthorObj.name) &&
                    birthDate.Equals(AuthorObj.birthDate) &&
                    books.Equals(AuthorObj.books)
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => name;
    }

    public class Publisher
    {
        public String name { get; set; }
        public String city { get; set; }
        public List<Book> books { get; set; }

        public Publisher(String name, String city, params Book[] books)
        {
            this.name = name;
            this.city = city;
            if (books != null)
                this.books = new List<Book>(books);
        }

        public override bool Equals(object obj)
        {
            Publisher PublisherObj = obj as Publisher;
            if (PublisherObj == null)
                return false;
            else
                return (
                    name.Equals(PublisherObj.name) &&
                    city.Equals(PublisherObj.city) &&
                    books.Equals(PublisherObj.books)
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => name + " from " + city;
    }
}