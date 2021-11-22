namespace Markdown.Models
{
    public interface IToken
    {
        public TagType TagType { get; }
        public ITokenPattern Pattern { get; }
        public ITagConverter TagConverter { get; }
    }
}