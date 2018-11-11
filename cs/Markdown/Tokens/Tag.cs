using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    class Tag : IToken
    {
        public string Text { get; }
        private List<IToken> tokens;
        private string tag;
        private string closeTag;

        public Tag(string text, string tag, List<IToken> tokens)
        {
            this.tag = tag;
            closeTag = tag.Insert(1, "/");
            Text = text;
            this.tokens = tokens;
        }

        public string GetContent()
        {
            var result = string.Concat(tokens.Aggregate(tag, (current, token) => current + token.GetContent()), closeTag);
            return result;
        }
    }
}
