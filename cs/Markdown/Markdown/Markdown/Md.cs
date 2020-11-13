using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Markdown
{
    public class Md
    {
        private Parser parser; 
        private MarkdownProcessor markdownProcessor;

        public Md()
        {
            markdownProcessor = new MarkdownProcessor();
            parser = new Parser(markdownProcessor.Marks);
        }
        public string Render(string text)
        {
            var tokens = parser.GetTokens(text);
            var formattedToken = markdownProcessor.FormatTokens(tokens);
            return GetHTML(formattedToken);
            throw new NotImplementedException();
        }

        private string GetHTML(List<TokenMd> formattedTokens)
        {
            // склеевает текст назад
            throw new NotImplementedException();
        }
    }
}