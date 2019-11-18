using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.MdTags;
using Markdown.Parser;

namespace Markdown.MarkDown
{
    public class MdProcessor
    {
        private readonly MdTagParser parser;

        public string Render(string rawMarkdown)
        {
            var paragraphs = rawMarkdown.Split(new[] { "\n\n" }, StringSplitOptions.None);
            var paragraphsTags = paragraphs.Select(paragraph => CombineTagsInOneParagraph(parser.Parse(paragraph))).ToList();
            return CombineAllParagraphs(paragraphsTags);
        }

        public MdProcessor(IParser<Tag> parser)
        {
            this.parser = new MdTagParser();
        }

        private string CombineTagsInOneParagraph(List<Tag> tags) 
            => string.Join("", tags.Select(tag => tag.WrapTagIntoHtml()));

        private string CombineAllParagraphs(List<string> paragraphs) 
            => string.Join("", paragraphs.Select(paragraph => $"<p>{paragraphs}</p>"));
    }
}
