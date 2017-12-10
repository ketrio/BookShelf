using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookShelf.Pages
{
    class PageLoader : INotifyPropertyChanged
    {
        private readonly Type type;

        int position;
        private List<object> _collection;

        private bool _prevActive = false;
        private bool _nextActive = false;
        private bool _boundActive = false;

        public bool PrevActive
        {
            get => _prevActive;
            set
            {
                _prevActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrevActive"));
            }
        }
        public bool NextActive
        {
            get => _nextActive;
            set
            {
                _nextActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NextActive"));
            }
        }
        public bool BoundActive {
            get => _boundActive;
            set {
                _boundActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundActive"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                EvalButtons();
                Update();
            }
        }

        public Page First { get; private set; } = null;
        public Page Last { get; private set; } = null;
        public Page Prev { get; private set; } = null;
        public Page Next { get; private set; } = null;

        public List<object> Collection { get => _collection; set { _collection = value; Position = 0; } }

        public PageLoader(List<object> collection, Type type)
        {
            if (!typeof(Page).IsAssignableFrom(type)) throw new ArgumentException();
            Collection = collection;
            this.type = type;
            Position = 0;

            Update();
            EvalButtons();
        }

        void EvalButtons()
        {
            NextActive = Position < Collection.Count - 1;
            PrevActive = Position > 0;
            BoundActive = Collection.Count > 0;
        }

        async void Update()
        {
            await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                First = Activator.CreateInstance(type, Collection.FirstOrDefault()) as Page;
                Last = Activator.CreateInstance(type, Collection.LastOrDefault()) as Page;
                Prev = Activator.CreateInstance(type, Collection.ElementAtOrDefault(Position - 1)) as Page;
                Next = Activator.CreateInstance(type, Collection.ElementAtOrDefault(Position + 1)) as Page;
            }));
        }
    }
}
