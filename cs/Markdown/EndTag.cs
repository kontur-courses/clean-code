namespace Markdown
{
    public class EndTag : Tag
    {
        public EndTag(TextSelectionType selectionType, int position, int length) : base(selectionType, position, length)
        {
        }
    }
}