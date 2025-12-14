using System.Collections.Generic;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Iterator
{
    public class LogCollection : IAggregate
    {
        private List<ResourceLog> _items = new List<ResourceLog>();

        public void Add(ResourceLog item)
        {
            _items.Add(item);
        }

        public IIterator CreateIterator()
        {
            return new LogIterator(this);
        }

        public int Count => _items.Count;

        public object this[int index]
        {
            get => _items[index];
            set => _items[index] = (ResourceLog)value;
        }
    }
}