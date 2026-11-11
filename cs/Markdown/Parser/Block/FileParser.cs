using Markdown.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Block
{
    public static class FileParser
    {
        public static IEnumerable<string> SplitTextForParagraph(string sourceText)
        {
            if (string.IsNullOrWhiteSpace(sourceText))
                yield break;

            var normalizedText = sourceText.Replace("\r\n", "\n").Replace("\r", "\n");
            var blocks = normalizedText.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var block in blocks)
            {
                var trimmedBlock = block.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedBlock))
                {
                    yield return trimmedBlock;
                }
            }
        }
    }
}
