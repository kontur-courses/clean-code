using System;

namespace Markdown
{
    internal class MarkdownToHtmlParser
    {
        public string Render(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"nameof{input} can't be null or empty");

            var markdownParser = new MarkdownToTokenParser();
            var tokens = markdownParser.ParseToTokens(input);
            return TokenToHtmlParser.GetHtmlTextFromTokens(tokens);
        }
    }
}