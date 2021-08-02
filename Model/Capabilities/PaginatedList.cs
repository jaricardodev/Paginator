using System.Collections;
using System.Collections.Generic;

namespace Jaricardodev.Paginator.Model.Capabilities
{
    public class PaginatedList<T> : IList<T>, IPaginatedList
    {
        private readonly List<T> _internalList;
        public int TotalItemsCount { get; set; }
        public int TotalPageCount { get; set; }

        public PaginatedList()
        {
            _internalList = new List<T>();
        }

        private PaginatedList(List<T> page)
        {
            _internalList = page;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        public static implicit operator PaginatedList<T>(List<T> page)
        {
            return new PaginatedList<T>(page);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _internalList.Add(item);
        }

        public void Clear()
        {
            _internalList.Clear();
        }

        public bool Contains(T item)
        {
            return _internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _internalList.Remove(item);
        }

        public int Count => _internalList.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return _internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _internalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _internalList.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _internalList[index];
            set => _internalList[index] = value;
        }
    }
}
