using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class CollectionView<T>
    {
        public T[] SourceCollection { get; }
        public int Position { get; set; }

        public CollectionView(T[] sourceCollection, int position)
        {
            SourceCollection = sourceCollection;
            Position = position;
        }

        public bool TryGetValue(int i, out T value)
        {
            value = default(T);
            var index = i + Position;
            if (index < 0 || index >= SourceCollection.Length)
                return false;
            value = SourceCollection[i + Position];
            return true;
        }

        public T GetValue(int i)
        {
            var index = i + Position;
            if (index < 0 || index >= SourceCollection.Length)
                throw new IndexOutOfRangeException($"Resulting index {index} = (position {Position} + {i}) was out of range");
            return SourceCollection[i + Position];
        }

        public ElementContext<T> Element(int i)
        {
            return new ElementContext<T>(this, i);
        }
    }
}
