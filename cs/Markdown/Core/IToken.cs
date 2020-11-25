namespace Markdown.Core
{
    public interface IToken
    {
        public string MdTag { get; }
        public int MdTokenLength { get; }
        public string ToHtmlString();
    }
}