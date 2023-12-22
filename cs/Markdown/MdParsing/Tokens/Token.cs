using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Content { get; }
        public TagType TagType { get; }
        public bool IsCloseTag { get; set; }
        public int PairedTagIndex { get; set; }

        public Token(TokenType tokenType, string content, TagType tagType = TagType.Unexpected)
        {
            TokenType = tokenType;
            Content = content;
            TagType = tagType;
        }
    }
}
