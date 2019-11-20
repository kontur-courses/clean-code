using System.Collections.Generic;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Markdowns
{
    public class XmlMarkdown : IResultMarkdown
    {
        public Dictionary<TokenType, string> OpeningTags =>
            new Dictionary<TokenType, string>
            {
                {TokenType.Bold, "<bold>"},
                {TokenType.Italic, "<italic>"},
                {TokenType.PlainText, ""},
                {TokenType.Parent, "<xml>"}
            };

        public Dictionary<TokenType, string> ClosingTags =>
            new Dictionary<TokenType, string>()
            {
                {TokenType.Bold, "</bold>"},
                {TokenType.Italic, "</italic>"},
                {TokenType.PlainText, ""},
                {TokenType.Parent, "</xml>"}
            };
    }
}