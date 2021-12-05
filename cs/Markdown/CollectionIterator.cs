using System;
using System.Collections.Generic;
using System.Diagnostics;
using Markdown.Tokens;

namespace Markdown
{
    public class CollectionIterator<TElementType>
    {
        private readonly List<TElementType> collection;
        private int position;

        public bool IsFinished => position >= collection.Count;

        public CollectionIterator(IEnumerable<TElementType> enumerable)
        {
            this.collection = new List<TElementType>(enumerable);
        }
        public void Move(int delta)
        {
            if (delta <= 0)
                throw new Exception($"{nameof(delta)} must be positive");
            position += delta;
        }

        public TElementType GetCurrent()
        {
            return collection[position];
        }
        
        public bool TryGet(int delta, out TElementType token)
        {
            if (collection.InBorders(position + delta))
            {
                token = collection[position + delta];
                return true;
            }

            token = default;
            return false;
        }
    }
}