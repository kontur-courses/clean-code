using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        private MarkupParser markupParser;
        private MarkupBuilder htmlBuilder;
        
        public string Render(string rawText)
        {
            var tags = CreateMdToHtmlTags();
            markupParser = new MarkupParser(tags);
            htmlBuilder = new MarkupBuilder(tags);
            // Сперва разберем с помощью парсера, потом соберем с помощью сборщика
            throw new NotImplementedException();
        }

        private TagData[] CreateMdToHtmlTags()
        {
            throw new NotImplementedException();
        }
    }
}