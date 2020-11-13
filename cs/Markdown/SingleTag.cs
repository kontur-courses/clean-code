namespace Markdown
{
    public class SingleTag : Tag

    {
        public SingleTag(TextSelectionType selectionType, int position, int length) : base(selectionType, position,
            length)
        {
        }
    }
}