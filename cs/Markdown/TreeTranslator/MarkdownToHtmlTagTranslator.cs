using System;
using System.Collections.Generic;
using Markdown.Data;

namespace Markdown.TreeTranslator
{
    public class MarkdownToHtmlTagTranslator : ITagTranslator
    {
        private readonly Dictionary<string, string> translations = new Dictionary<string, string>();

        public MarkdownToHtmlTagTranslator(IEnumerable<TagTranslationInfo> translations)
        {
            foreach (var translation in translations)
            {
                this.translations[translation.OpeningTag] = translation.Translation;
                this.translations[translation.ClosingTag] = translation.Translation;
            }
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