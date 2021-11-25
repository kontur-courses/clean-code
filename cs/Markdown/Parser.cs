using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        private readonly ParagraphParser paragraphParser = new ParagraphParser();

        public IEnumerable<Token> Parse(string mdText)
        {
            if (string.IsNullOrEmpty(mdText))
                throw new ArgumentException("Text should be not null and not empty");
            var paragraphs = SplitByParagraphs(mdText);
            foreach (var paragraph in paragraphs)
            {
                yield return GetParagraphOrHeader(paragraph);
            }
        }

        private string[] SplitByParagraphs(string mdText)
            => mdText.Split("\r\n");

        private Token GetParagraphOrHeader(string text)
        {
            Token token;
            if (text.Length > 0 && text[0] == '#')
                token = new Header(text.Substring(1, text.Length - 1));
            else
                token = new Paragraph(text);
            token.InnerTokens = paragraphParser.TokenizeParagraph(token.Value);
            return token;
        }
    }
}
