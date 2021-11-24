using Markdown.Tags;

namespace Markdown.Tokens
{
    public class Token
    {
        public TagType TagType { get; }
        public TagRole TagRole { get; set; }
        public TokenType TokenType { get; private set; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length - 1;

        public Token(TokenType tokenType, TagType tagType, TagRole tagRole, int start, int length)
        {
            TagType = tagType;
            Start = start;
            Length = length;
            TagRole = tagRole;
            TokenType = tokenType;
        }

        public Token(TokenType tokenType, int start, int length)
        {
            Length = length;
            TokenType = tokenType;
            Start = start;
        }

        public void SwitchToText()
        {
            TokenType = TokenType.Text;
        }
    }
}