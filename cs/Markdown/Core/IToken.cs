namespace Markdown.Core
{
    public interface IToken
    {
        public int MdTokenLength { get; }
        public string ToHtmlString();
    }
}