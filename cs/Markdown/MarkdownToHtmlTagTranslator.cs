using System;

namespace Markdown
{
    public class MarkdownToHtmlTagTranslator : ITagTranslator
    {
        private readonly MarkdownTagLanguage language;

        public MarkdownToHtmlTagTranslator(MarkdownTagLanguage language)
        {
            this.language = language;
        }

        public string Translate(string tagBody, string tagSymbol)
        {
            throw new NotImplementedException();
        }

        private string BuildOpeningTag(string tagSymbol)
        {
            throw new NotImplementedException();
        }

        private string BuildClosingTag(string tagSymbol)
        {
            throw new NotImplementedException();
        }
    }
}