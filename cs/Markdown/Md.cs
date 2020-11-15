using System;
using System.Collections.Generic;
using Markdown.Tag;
using Markdown.Parser;
using Markdown.Builder;

namespace Markdown
{
    public class Md
    {
        private MarkupParser markdownParser;
        private MarkupBuilder htmlMarkupBuilder;

        public Md()
        {
            var tags = CreateMdToHtmlTags();
            markdownParser = new MarkupParser(tags);
            htmlMarkupBuilder = new MarkupBuilder(tags);
        }
        
        public string Render(string rawText)
        {
            var textTokens = markdownParser.Parse(rawText);
            return htmlMarkupBuilder.Build(textTokens);
        }

        private TagData[] CreateMdToHtmlTags()
        {
            throw new NotImplementedException();
        }
    }
}