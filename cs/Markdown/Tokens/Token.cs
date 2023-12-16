using Markdown.Tags;

namespace Markdown.Tokens
{
    public class Token : IToken
    {
        public Token(string content, int start, int length, Tag tag)
        { 
            Content = content;
            Start = start;
            Length = length;
            Tag = tag;
        }

        public Tag Tag { get; }
        public string Content { get; }
        public int Start { get; }
        public int Length { get; }
    }
}
