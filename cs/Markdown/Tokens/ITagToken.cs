namespace Markdown
{
    public interface ITagToken : IToken
    {
        public string TextWithoutTags { get; }
    }
}