using Markdown.Tags;

namespace Markdown.Tokens
{
    public interface IToken
    {
        public TagType Tag { get; }
        public string Content { get; }
        public int Start { get; }
        public int Length { get; }
    }
}
