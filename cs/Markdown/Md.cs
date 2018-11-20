using System.Collections.Generic;
using Markdown.Readers;

namespace Markdown
{
    public class Md
    {
        private readonly Parser parser;
        private readonly HtmlRenderer htmlRenderer;

        public Md()
        {
            var readingOptions = GetReadingOptionsForParser();
            parser = new Parser(readingOptions);
            htmlRenderer = new HtmlRenderer();
        }

        public string RenderToHtml(string mdText)
        {
            var mdTokens = parser.Parse(mdText);
            return htmlRenderer.Render(mdTokens);
        }

        private ReadingOptions GetReadingOptionsForParser()
        {
            var emReader = new PairedTagReader("em", "_");
            var strongReader = new PairedTagReader("strong", "__");
            var backslashReader = new BackslashReader();
            var textReader = new TextReader("_\\");
            var anyCharReader = new AnyCharReader();

            var allowedReaders = new List<AbstractReader> {
                strongReader, emReader, backslashReader, textReader, anyCharReader };
            var mutedReaders = new Dictionary<AbstractReader, HashSet<AbstractReader>> {
                [emReader] = new HashSet<AbstractReader> { backslashReader, textReader, anyCharReader },
                [strongReader] = new HashSet<AbstractReader> { emReader, backslashReader, textReader, anyCharReader }
            };

            return new ReadingOptions(allowedReaders, mutedReaders);
        }
    }
}