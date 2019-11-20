using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{

    class FixedSizeQueue<T>
    {
        private LinkedList<T> Elements;
        public int Size { get; }
        int Count;

        public FixedSizeQueue(int size)
        {
            Elements = new LinkedList<T>();
            Size = size;
        }

        public bool IsEmpty { get { return Elements.Count==0; } }

        public void Enqueue(T value)
        {
            Elements.AddLast(value);
            while (Elements.Count > Size)
                Elements.RemoveFirst();
        }

        public T Get(int index)
        {
            if (Elements.Count <= index)
                return default;
            return Elements.ElementAt(index);
        }

        public T Last()
        {
            if (Elements.Last == null)
                return default;
            return Elements.Last.Value;
        }
    }
}
