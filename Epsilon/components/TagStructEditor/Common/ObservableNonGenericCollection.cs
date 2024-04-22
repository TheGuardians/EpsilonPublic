using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace TagStructEditor.Common
{
    [DoNotNotify]
    public class ObservableNonGenericCollection : IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private IList _collection;

        public IList BaseCollection
        {
            get => _collection;
            set
            {
                _collection = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                NotifyOfPropertyChange(nameof(BaseCollection));
                NotifyOfPropertyChange(nameof(Count));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableNonGenericCollection()
        {
            _collection = new ArrayList();
        }

        public ObservableNonGenericCollection(IEnumerable enumerable)
        {
            _collection = enumerable.Cast<object>().ToList();
        }

        public ObservableNonGenericCollection(IList collection)
        {
            _collection = collection;
        }

        public bool IsReadOnly => _collection.IsReadOnly;

        public bool IsFixedSize => _collection.IsFixedSize;

        public int Count => _collection.Count;

        public object SyncRoot => _collection.SyncRoot;

        public bool IsSynchronized => _collection.IsSynchronized;

        public object this[int index]
        {
            get => _collection[index];
            set => _collection[index] = value;
        }

        public bool Contains(object value) => _collection.Contains(value);

        public int IndexOf(object value) => _collection.IndexOf(value);

        public void CopyTo(Array array, int index) => _collection.CopyTo(array, index);

        public IEnumerator GetEnumerator() => _collection.GetEnumerator();

        public int Add(object value)
        {
            var index = _collection.Add(value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
            NotifyOfPropertyChange(nameof(Count));
            return index;
        }

        public void Clear()
        {
            _collection.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyOfPropertyChange(nameof(Count));
        }

        public void Insert(int index, object value)
        {
            _collection.Insert(index, value);
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
            NotifyOfPropertyChange(nameof(Count));
        }

        public void Remove(object value)
        {
            var index = _collection.IndexOf(value);
            if (index < 0)
                return;

            _collection.Remove(value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
            NotifyOfPropertyChange(nameof(Count));
        }

        public void RemoveAt(int index)
        {
            var value = _collection[index];
            _collection.RemoveAt(index);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
            NotifyOfPropertyChange(nameof(Count));
        }

        public void Move(int oldIndex, int newIndex)
        {
            var value = _collection[oldIndex];
            _collection.RemoveAt(oldIndex);
            _collection.Insert(newIndex, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, value, newIndex, oldIndex));
        }

        private void NotifyOfPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
