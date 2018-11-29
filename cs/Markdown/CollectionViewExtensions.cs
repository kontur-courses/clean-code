namespace Markdown
{
    public static class CollectionViewExtensions
    {
        public static bool IsWhiteSpace(this CollectionView<char> collectionView, int index)
        {
            collectionView.TryGetValue(index, out var value);
            return char.IsWhiteSpace(value);
        }

        public static bool IsUnderscore(this CollectionView<char> collectionView, int index)
        {
            collectionView.TryGetValue(index, out var value);
            return value == '_';
        }

        public static bool IsAlphanumeric(this CollectionView<char> collectionView, int index)
        {
            collectionView.TryGetValue(index, out var value);
            return char.IsLetterOrDigit(value);
        }

        public static bool IsEscapeSymbol(this CollectionView<char> collectionView, int index)
        {
            collectionView.TryGetValue(index, out var value);
            return value == '\\';
        }
    }
}
