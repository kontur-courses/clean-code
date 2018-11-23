using System;
using System.Collections.Generic;
using System.Linq;
using MarkDown.TagTypes;

namespace MarkDown
{
    public class Token
    {
        public int Position { get; }
        
        public TokenType TokenType => TagType == null ? TokenType.Text : TokenType.Tag; 

        public TagType TagType { get; }

        private List<Character> ParamContent { get; }

        public List<Character> Content { get; }
        
        public int Length => Content.Count + (TagType?.OpeningSymbol.Length + TagType?.ClosingSymbol.Length ?? 0);

        public IEnumerable<Token> InnerTokens { get; set; }

        public Token(int position, List<Character> content, TagType tagType = null, List<Character> paramContent = null)
        {
            Position = position;
            Content = content ?? new List<Character>();
            TagType = tagType;
            ParamContent = paramContent;
            InnerTokens = new List<Token>();
        }

        public string ToHtml(string content)
        {
            var escapedParamContent = ParamContent == null ? string.Empty : string.Concat(ParamContent.Where(s => s.CharState != CharState.Ignored).Select(s => s.Char));
            return TagType == null ? content : TagType.ToHtml(content, escapedParamContent);
        }
    }
}