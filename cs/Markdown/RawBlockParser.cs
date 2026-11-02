using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class RawBlockParser
    {
        public static RawBlock[] Parse(string text)
        {
            throw new NotImplementedException();
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
