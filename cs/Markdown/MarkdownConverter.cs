using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkdownConverter
    {
        private readonly Dictionary<string, HtmlTag> markdownToHtmlDictionary;

        public MarkdownConverter()
        {
            markdownToHtmlDictionary = new Dictionary<string, HtmlTag>
            {
                {"#", new HtmlTag("h1")},
                {"\n", new HtmlTag("h1")},
                {"_", new HtmlTag("em")},
                {"__", new HtmlTag("strong")},
            };
        }

        public string ConvertToHtml(string markdown)
        {
            var replacements = FindReplacements(markdown);
            return ReplaceTags(markdown, replacements);
        }

        private TagReplacement[] FindReplacements(string markdown)
        {
            throw new NotImplementedException();
        }

        private string ReplaceTags(string markdown, TagReplacement[] replacements)
        {
            var resultBuilder = new StringBuilder();
            throw new NotImplementedException();
        }
    }
}