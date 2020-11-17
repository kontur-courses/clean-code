namespace Markdown.Tags.ItalicTag
{
    public class ItalicTag : Tag
    {
        public const int TagLength = 1;
        
        public ItalicTag(string value, int index) : base(value, index, TagLength)
        {
        }
    }
}