namespace Markdown.Tags.ItalicTag
{
    public class OpenItalicTag : ItalicTag
    {
        public OpenItalicTag(int index) : base("<em>", index)
        {
        }
    }
}