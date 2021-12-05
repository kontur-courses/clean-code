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
        public bool ElementIsLast => position + 1 == collection.Count;

        public CollectionIterator(IEnumerable<TElementType> enumerable)
        {
            this.collection = new List<TElementType>(enumerable);
        }
        public void Move(int deltaPosition)
        {
            if (deltaPosition <= 0)
                throw new Exception($"{nameof(deltaPosition)} must be positive");
            position += deltaPosition;
        }

        public TElementType GetCurrent()
        {
            return collection[position];
        }
        
        public bool TryGet(int deltaPosition, out TElementType element)
        {
            if (collection.InBorders(position + deltaPosition))
            {
                element = collection[position + deltaPosition];
                return true;
            }

            element = default;
            return false;
        }
    }
}