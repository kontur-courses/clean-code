namespace Markdown
{
    public abstract class Tag
    {
        public readonly int Position;
        public readonly TextSelectionType SelectionType;
        public readonly int Length;

        protected Tag(TextSelectionType selectionType, int position, int length)
        {
            Position = position;
            SelectionType = selectionType;
            Length = length;
        }
    }
}