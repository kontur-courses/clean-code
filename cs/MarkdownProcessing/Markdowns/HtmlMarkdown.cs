using System.Collections.Generic;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Markdowns
{
    public struct HtmlMarkdown : IResultMarkdown
    {
        public Dictionary<TokenType, string> OpeningTags =>
            new Dictionary<TokenType, string>
            {
                {TokenType.Bold, "<strong>"},
                {TokenType.Italic, "<em>"},
                {TokenType.PlainText, ""},
                {TokenType.Parent, "<p>"}
            };

        public Dictionary<TokenType, string> ClosingTags =>
            new Dictionary<TokenType, string>()
            {
                {TokenType.Bold, "</strong>"},
                {TokenType.Italic, "</em>"},
                {TokenType.PlainText, ""},
                {TokenType.Parent, "</p>"}
            };
    }
}