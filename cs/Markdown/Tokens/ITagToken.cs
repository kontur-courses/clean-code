namespace Markdown
{
    public interface ITagToken : IToken
    {
        public string TextWithTags { get; }
    }
}