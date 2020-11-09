using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TextParser
    {
        public string ParseText(string text)
        {
            var tokens = new List<Token>();
            var parsedText = new List<string>();

            foreach (var paragraph in GetParagraphs(text))
            {
                parsedText.Add(ParseParagraph(paragraph));
            }

            return parsedText.ToString();
        }

        private IEnumerable<string> GetParagraphs(string text)
        {
            throw new NotImplementedException();
        }

        private string ParseParagraph(string paragraph)
        {
            var htmlConverter = new HtmlConverter();

            throw new NotImplementedException();
        }

        private string RemoveBackSlashes(string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Token> GetTokens(string paragraph)
        {
            throw new NotImplementedException();
        }
    }
}