using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Interfaces;
using Markdown.PairTags;

namespace Markdown
{
    public class MarkdownRenderer
    {
        private readonly ITranslator translator;

        public MarkdownRenderer(ITranslator translator)
        {
            this.translator = translator;
        }

        public string Render(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var tags = new List<ITag>();
            foreach (var paragraph in text.Split(Environment.NewLine))
            {
                tags.AddRange(RenderParagraph(paragraph));
                tags.Add(new Word(Environment.NewLine));
            }

            if (tags.Count == 0)
                return translator.Translate(Enumerable.Empty<ITag>());

            tags.RemoveAt(tags.Count - 1);
            return translator.Translate(tags);
        }

        private static IEnumerable<ITag> RenderParagraph(string paragraph)
        {
            var result = new List<ITag>();
            foreach (var word in paragraph.Split(' '))
            {
                result.AddRange(WordAnalyzer.SplitWordIntoTags(word));
                result.Add(new Word(" "));
            }

            if (result.Count == 0)
                return Enumerable.Empty<ITag>();

            result.RemoveAt(result.Count - 1);

            if (result.Count != 0 && result[0].Tag == Tag.Header)
                result.Add(new ClosingHeader());

            return AnalyzerSequence.AnalyzeSequence(result);
        }
    }
}