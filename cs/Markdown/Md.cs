using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdown.MarkdownConfigurations;
using Markdown.TextProcessing;

namespace Markdown
{
    public class Md
    {
        public TextBuilder Builder { get;}
        public IConfig MdConfiguration { get; }

        public Md(IConfig config)
        {
            MdConfiguration = config;
            Builder = new TextBuilder(MdConfiguration);
        }

        public string Render(string content)
        {
            var paragraphs = content.Split(new[] { "\r\n\r\n" }, StringSplitOptions.None)
                .Select((t, i) => (t, i));
            var processedParagraphs = ParallelRender(paragraphs);
            var htmlCode = Builder.BuildText(processedParagraphs);
            return htmlCode;
        }

        public string RenderOneParagraph(string paragraph)
        {
            var splitter = new TextSplitter(paragraph,Builder);
            var tokens = splitter.SplitToTokens();
            var htmlCode = Builder.BuildText(tokens);
            return htmlCode;
        }

        private List<string> ParallelRender(IEnumerable<(string, int)> paragraphs)
        {
            var processedParagraphs = new ConcurrentBag<(string Paragraph, int NamberParagraph)>();
            var pRes = Parallel.ForEach(paragraphs, paragraph =>
            {
                var processedParagraph = RenderOneParagraph(paragraph.Item1);
                processedParagraphs.Add((processedParagraph, paragraph.Item2));
            });
            if (!pRes.IsCompleted)
                throw new Exception("Parallel error");
            return processedParagraphs.OrderBy(paragraph => paragraph.NamberParagraph).Select(paragraph => paragraph.Paragraph)
                .ToList();
        }
    }
}
