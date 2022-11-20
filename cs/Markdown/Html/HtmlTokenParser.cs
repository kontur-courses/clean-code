using System.Collections.Generic;
using Markdown.Interfaces;
using Markdown.Tags.Html;

namespace Markdown.Html
{
    public class HtmlTokenParser : ITokenParser
    {
        private readonly List<Token> tokens;

        public HtmlTokenParser()
        {
            tokens = new List<Token>();
        }

        public IEnumerable<Token> Parse(string data)
        {
            // ...
            return tokens;
        }
    }
}