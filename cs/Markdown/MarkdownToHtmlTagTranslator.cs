using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownToHtmlTagTranslator : ITagTranslator
    {
        private readonly Dictionary<string, string> translations;

        public MarkdownToHtmlTagTranslator(Dictionary<string, string> translations)
        {
            this.translations = translations;
        }

        public string TranslateOpeningTag(string tag) => BuildTag("<", tag, ">");

        public string TranslateClosingTag(string tag) => BuildTag("</", tag, ">");

        private string BuildTag(string leftEdge, string tag, string rightEdge)
        {
            if (translations.TryGetValue(tag, out var translation))
                return leftEdge + translation + rightEdge;
            throw new ArgumentException("No such tag in translations dictionary");
        }
    }
}