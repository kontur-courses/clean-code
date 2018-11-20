using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdown.TextProcessing;

namespace Markdown
{
    public class Md
    {
        public TextBuilder Builder { get; set; }
        public Md()
        {
            Builder = new TextBuilder();
        }
        public string Render(string content)
        {
            var paragraphs = ParagraphsNumbering(content.Split(new[] { "\r\n\r\n" }, StringSplitOptions.None));
            var processedParagraphs = ParallelRender(paragraphs);
            var htmlCode = Builder.BuildText(processedParagraphs);
            return htmlCode;
        }

        public string RenderOneParagraph(string paragraph)
        {
            var splitter = new TextSplitter(paragraph);
            var tokens = splitter.SplitToTokens();
            var htmlCode = Builder.BuildText(tokens);
            return htmlCode;
        }

        private List<string> ParallelRender(IEnumerable<Tuple<string, int>> paragraphs)
        {
            var processedParagraphs = new ConcurrentBag<Tuple<string, int>>();
            var pRes = Parallel.ForEach(paragraphs, paragraph =>
            {
                var processedParagraph = RenderOneParagraph(paragraph.Item1);
                processedParagraphs.Add(Tuple.Create(processedParagraph, paragraph.Item2));
            });
            if (!pRes.IsCompleted)
                throw new Exception("Parallel error");
            return processedParagraphs.OrderBy(paragraph => paragraph.Item2).Select(paragraph => paragraph.Item1)
                .ToList();
        }

        private IEnumerable<Tuple<string, int>> ParagraphsNumbering(string[] paragraphs)
        {
            var result = new Tuple<string, int>[paragraphs.Length];
            for (int i = 0; i < paragraphs.Length; i++)
            {
                result[i] = Tuple.Create(paragraphs[i], i);
            }

            return result;
        }
    }
}
