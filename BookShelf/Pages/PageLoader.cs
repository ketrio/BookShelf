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
    public class PageLoader : INotifyPropertyChanged
    {
        private readonly Type type;

        int position;
        private List<object> _collection;

        private bool _prevActive = false;
        private bool _nextActive = false;

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

        public int LastIndex { get => _collection.Count - 1; }

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
        
        public PageLoader(List<object> collection, Type type)
        {
            if (!typeof(Page).IsAssignableFrom(type)) throw new ArgumentException();
            _collection = collection;
            this.type = type;
            Position = 0;

            Update();
            EvalButtons();
        }

        void EvalButtons()
        {
            NextActive = Position < _collection.Count - 1;
            PrevActive = Position > 0;
        }

        async void Update()
        {
            await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                First = Activator.CreateInstance(type, _collection.FirstOrDefault()) as Page;
                Last = Activator.CreateInstance(type, _collection.LastOrDefault()) as Page;
                Prev = Activator.CreateInstance(type, _collection.ElementAtOrDefault(Position - 1)) as Page;
                Next = Activator.CreateInstance(type, _collection.ElementAtOrDefault(Position + 1)) as Page;
            }));
        }
    }
}
