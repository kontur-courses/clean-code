using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown.Tokens
{
    class Tag : IToken
    {
        public string Text { get; }
        public int Position { get; }
        private List<IToken> tokens;
        private string htmlTag;
        private string closingHtmlTag;

        public Tag(string text, string htmlTag, List<IToken> tokens, int position)
        {
            this.htmlTag = htmlTag;
            closingHtmlTag = htmlTag.Insert(1, "/");
            Text = text;
            this.tokens = tokens;
            this.Position = position;
        }

        public string ToHtml()
        {
            var result = string.Concat(tokens.Aggregate(htmlTag, (current, token) => current + token.ToHtml()), closingHtmlTag);
            return result;
        }
    }
}
