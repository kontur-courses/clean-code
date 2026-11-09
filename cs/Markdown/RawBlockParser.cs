using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class RawBlockParser
    {
        public static RawBlock[] Parse(string text)
        {

            if (string.IsNullOrEmpty(text))
                return Array.Empty<RawBlock>();

            var blocks = new List<RawBlock>();
            var paragraphBuffer = new List<string>();

            foreach (var rawLine in text.Split('\n'))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    FlushParagraph(paragraphBuffer, blocks);
                    continue;
                }

                if (line.Length >= 2 && line[0] == '#' && line[1] == ' ')
                {
                    FlushParagraph(paragraphBuffer, blocks);
                    blocks.Add(new RawBlock(BlockType.Heading, line.Substring(2)));
                }
                else
                    paragraphBuffer.Add(line);
            }

            FlushParagraph(paragraphBuffer, blocks);
            return blocks.ToArray();
        }

        private static void FlushParagraph(List<string> buffer, List<RawBlock> blocks)
        {
            if (buffer.Count == 0) return;
            blocks.Add(new RawBlock(BlockType.Paragraph, string.Join(" ", buffer)));
            buffer.Clear();
        }
    }

    public class RawBlock
    {
        public BlockType Type { get; }
        public string Content { get; }

        public RawBlock(BlockType type, string content)
        {
            Type = type;
            Content = content;
        }
    }

    public enum BlockType
    {
        Heading,
        Paragraph
    }
}
