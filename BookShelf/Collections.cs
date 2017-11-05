using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Models;

namespace Collections
{
    public class MyCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        protected T[] _items;
        private int _size;
        private int _gen;

        static readonly T[] _emptyArray = new T[0];

        public MyCollection()
        {
            _items = _emptyArray;
        }

        public MyCollection(int size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException();

            if (size == 0) _items = _emptyArray;
            else _items = new T[size];
        }

        public MyCollection(params T[] items) : this(items.Length)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public T this[int index] {
            get
            {
                if (index >= _size || index < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _items[index];
            }

            set
            {
                if ((uint)index >= (uint)_size || index < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }


                _items[index] = value;
                _gen++;
            }
        }

        public int Count => _items.Length;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
            _gen++;
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _gen++;
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                    if (_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                for (int i = 0; i < _size; i++)
                {
                    if (item.Equals(_items[i])) return true;
                }
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {

            if ((array != null) && (array.Rank != 1))
            {
                throw new ArgumentException();
            }
            
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        public void Insert(int index, T item)
        {
            if (index > _size || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;
            _gen++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= _size || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default(T);
            _gen++;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (T _item in _items)
            {
                if (_item == null)
                {
                    break;
                }
                
                yield return _item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T _item in _items)
            {
                if (_item == null)
                {
                    break;
                }

                yield return _item;
            }
        }

        private class BookEnumerator : IEnumerator<T>
        {
            private readonly long _gen;
            private readonly MyCollection<T> _this;
            private int index;
            private T current;

            BookEnumerator(MyCollection<T> collection)
            {
                _gen = collection._gen;
                _this = collection;
                index = 0;
                current = default(T);
            }

            public T Current => current;

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (_this._gen != _gen)
                {
                    throw new InvalidOperationException("Collection was modified.");
                }

                if (index < _this._size)
                {
                    current = _this._items[index];
                    index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                index = 0;
                current = default(T);
            }
        }

        private void EnsureCapacity(int size)
        {
            if (_items.Length < size)
            {
                int newCapacity = _items.Length == 0 ? 2 : _items.Length * 2;
                if (newCapacity < size) newCapacity = size;
                Capacity = newCapacity;
            }
        }

        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public override string ToString()
        {
            if (_size == 0)
            {
                return "MyCollection";
            }
            else
            {
                String output = "";
                foreach (var item in this)
                {
                    output += item.ToString() + "\n";
                }
                return output;
            }
        }
    }

    class BookCollection : MyCollection<Book>
    {
        public BookCollection() : base() { }
        public BookCollection(int size) : base(size) { }
        public BookCollection(params Book[] items) : base(items) { }

        public BookCollection getByTitle(String title)
        {
            if (title == "")
            {
                throw new ArgumentException();
            }

            BookCollection booksByName = new BookCollection();
            foreach (Book book in _items)
            {
                if (book.title == title)
                {
                    booksByName.Add(book);
                }
            }

            return booksByName;
        }

        public BookCollection getByPeriod(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException();
            }

            BookCollection booksByPeriod = new BookCollection();
            foreach (Book book in _items)
            {
                DateTime publishDate = book.publishDate;
                if (start < publishDate && publishDate < end)
                {
                    booksByPeriod.Add(book);
                }
            }

            return booksByPeriod;
        }

        public BookCollection getByTag(String chosenTag)
        {
            if (chosenTag == "")
            {
                throw new ArgumentException();
            }

            BookCollection booksByTag = new BookCollection();
            foreach (Book book in _items)
            {
                String[] tags = book.tags;
                foreach (String tag in tags)
                {
                    if (tag == chosenTag)
                    {
                        booksByTag.Add(book);
                    }
                }
            }

            return booksByTag;
        }
    }

    class PubCollection : MyCollection<Publisher>
    {
        public PubCollection() : base() { }
        public PubCollection(int size) : base(size) { }
        public PubCollection(params Publisher[] items) : base(items) { }

        public PubCollection getByCity(String city)
        {
            if (city == "")
            {
                throw new ArgumentException();
            }

            PubCollection publishersByCity = new PubCollection();
            foreach (Publisher publisher in _items)
            {
                if (publisher.city == city)
                {
                    publishersByCity.Add(publisher);
                }
            }

            return publishersByCity;
        }
    }

    class AuthorCollection : MyCollection<Author>
    {
        public AuthorCollection() : base() { }
        public AuthorCollection(int size) : base(size) { }
        public AuthorCollection(params Author[] items) : base(items) { }

        public Author getByName(String name)
        {
            if (name == "")
            {
                throw new ArgumentException();
            }

            foreach (Author author in _items)
            {
                if (author.name == name)
                {
                    return author;
                }
            }

            return null;
        }

        public static implicit operator ObservableCollection<object>(AuthorCollection v)
        {
            throw new NotImplementedException();
        }
    }
}