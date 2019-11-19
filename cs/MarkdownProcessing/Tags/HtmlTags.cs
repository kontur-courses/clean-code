using System.Collections.Generic;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Tags
{
    public static class HtmlTags
    {
        public static readonly Dictionary<TokenType, string> HtmlOpeningTagsDictionary = new Dictionary<TokenType, string>
        {
            {TokenType.Bold, "<strong>"},
            {TokenType.Italic, "<em>"},
            {TokenType.PlainText, ""},
            {TokenType.Parent, "<p>"}
        };

        public static readonly Dictionary<TokenType, string> HtmlClosingTagsDictionary = new Dictionary<TokenType, string>
        {
            {TokenType.Bold, "</strong>"},
            {TokenType.Italic, "</em>"},
            {TokenType.PlainText, ""},
            {TokenType.Parent, "</p>"}
        };
    }
}