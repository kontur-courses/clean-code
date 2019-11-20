using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.MdTags;
using Markdown.Parser;

namespace Markdown.MarkDown
{
    public class MdProcessor
    {
        private readonly IParser<Tag> parser;

        public string Render(string rawMarkdown)
        {
            var paragraphs = rawMarkdown.Split(new[] { "\n\n" }, StringSplitOptions.None);
            var paragraphsTags = paragraphs.Select(paragraph => CombineTagsInOneParagraph(parser.Parse(paragraph))).ToList();
            return CombineAllParagraphs(paragraphsTags);
        }

        public MdProcessor(IParser<Tag> parser)
        {
            this.parser = parser;
        }

        private static string CombineTagsInOneParagraph(IEnumerable<Tag> tags) 
            => string.Join("", tags.Select(tag => tag.WrapTagIntoHtml()));

        private static string CombineAllParagraphs(IEnumerable<string> paragraphs) 
            => string.Join("", paragraphs.Select(paragraph => $"<p>{paragraph}</p>"));
    }
}
