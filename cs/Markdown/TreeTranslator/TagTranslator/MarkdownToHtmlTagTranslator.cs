using System;
using System.Collections.Generic;
using Markdown.Data;
using Markdown.Data.TagsInfo;

namespace Markdown.TreeTranslator.TagTranslator
{
    public class MarkdownToHtmlTagTranslator : ITagTranslator
    {
        private readonly Dictionary<(string, string), string> translations = new Dictionary<(string, string), string>();

        public MarkdownToHtmlTagTranslator(IEnumerable<TagTranslationInfo> translations)
        {
            foreach (var translation in translations)
                this.translations[(translation.OpeningTag, translation.ClosingTag)] = translation.Translation;
        }

        public TagTranslationResult Translate(ITagInfo tagInfo)
        {
            if (translations.TryGetValue((tagInfo.OpeningTag, tagInfo.ClosingTag), out var translation))
                return new TagTranslationResult($"<{translation}>", $"</{translation}>");
            throw new ArgumentException("No such tag in translations dictionary");
        }
    }
}