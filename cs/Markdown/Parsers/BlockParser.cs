using Markdown.Blocks;
using System.Collections.Generic;

namespace Markdown.Parsers
{
    public class BlockParser
    {
        private readonly InlineParser inlineParser = new();

        // Разбивает входной текст на блоки
        public List<IBlock> ParseToBlocks(string markdown)
        {
            var blocks = new List<IBlock>();
            if (string.IsNullOrEmpty(markdown)) return blocks;

            var lines = markdown.Split('\n');

            foreach (var line in lines)
            {
                string text = line.TrimEnd();
                if (string.IsNullOrEmpty(text)) continue;

                if (text.StartsWith("# "))
                {
                    var inlineTags = inlineParser.ParseInlineTags(text.Substring(2));
                    blocks.Add(new HeadingBlock(inlineTags));
                }
                else
                {
                    var inlineTags = inlineParser.ParseInlineTags(text);
                    blocks.Add(new ParagraphBlock(inlineTags));
                }
            }

            return blocks;
        }
    }
}
