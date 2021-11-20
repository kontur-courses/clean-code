using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown
{
    public class Token
    {
        public TagType Type { get; }
        public TagRole Role { get; }
        public TokenType TokenType { get; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length - 1;

        public Token(TagType type, int start, int length, TagRole role, TokenType tokenType)
        {
            Type = type;
            Start = start;
            Length = length;
            Role = role;
            TokenType = tokenType;
        }
    }
}