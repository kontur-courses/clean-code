namespace Markdown
{
    public class StartTag : Tag
    {
        public StartTag(TextSelectionType selectionType, int position, int length) : base(selectionType, position,
            length)
        {
        }
    }
}