namespace Markdown.Models
{
    public class Token : IToken
    {
        public TagType TagType { get; init; }
        public ITokenPattern Pattern { get; init; }
        public ITagConverter TagConverter { get; init; }
    }
}