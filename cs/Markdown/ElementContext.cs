using System;

namespace Markdown
{
    public class ElementContext<T>
    {
        private readonly CollectionView<T> collectionView;
        private readonly int index;

        public ElementContext(CollectionView<T> collectionView, int index)
        {
            this.collectionView = collectionView;
            this.index = index;
        }

        public bool Is(T other)
        {
            return collectionView.TryGetValue(index, out var value) && value.Equals(other);
        }

        public bool Is(Predicate<T> predicate)
        {
            return collectionView.TryGetValue(index, out var value) && predicate(value);
        }

        public bool IsNot(T other)
        {
            return !Is(other);
        }

        public bool IsNot(Predicate<T> predicate)
        {
            return !Is(predicate);
        }
    }
}
