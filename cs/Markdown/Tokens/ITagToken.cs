namespace Markdown.Tokens
{
    public interface ITagToken : IToken
    {
        public string TextWithoutTags { get; }
    }
}