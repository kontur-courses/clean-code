using System.Collections.Generic;
using MarkDown.TagTypes;

namespace MarkDown
{
    public class Token
    {
        public int Position { get; }
        
        public TokenType TokenType => TagType == null ? TokenType.Text : TokenType.Tag; 

        public TagType TagType { get; }

        public string ParamContent { get; }

        public string Content { get; }

        public int Length => Content.Length + (TagType?.OpeningSymbol.Length + TagType?.ClosingSymbol.Length ?? 0);

        public IEnumerable<Token> InnerTokens { get; set; }

        public Token(int position, string content, TagType tagType = null, string paramContent = "")
        {
            Position = position;
            Content = content;
            TagType = tagType;
            ParamContent = paramContent;
            InnerTokens = new List<Token>();
        }

        public string ToHtml(string content) => TagType == null ? content : TagType.ToHtml(content, ParamContent);
    }
}