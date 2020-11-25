using System;
using System.Linq;
using System.Text;
using Markdown;

namespace MarkdownTest
{
    public class MdMarkupGenerator
    {
        private string[] words = {"some", "random", "words", "for", "text", "generation"};
        private readonly MarkupProcessor markupProcessor;

        public MdMarkupGenerator(MarkupProcessor markupProcessor)
        {
            this.markupProcessor = markupProcessor;
        }

        public string GenerateMd(int wordsNumber)
        {
            var rnd = new Random();
            var sb = new StringBuilder();
            for (var i = 0; i < wordsNumber; i++)
            {
                var randomWord = words[rnd.Next() % words.Length];
                var randomTag = markupProcessor.AllTags.ToArray()[rnd.Next() % markupProcessor.AllTags.Count];
                sb.Append(randomTag);
                sb.Append(randomWord);
                sb.Append(randomTag);
                if (rnd.Next() % 30 == 0)
                    sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}