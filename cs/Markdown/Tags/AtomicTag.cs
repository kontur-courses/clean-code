namespace Markdown.Tokens
{
    public class AtomicTag : MdTag 
    {
        public string Content { get; set; }

        public AtomicTag(string content, TagType tagType)
            : base(tagType)
        {
            Content = content;
        }
    }
}