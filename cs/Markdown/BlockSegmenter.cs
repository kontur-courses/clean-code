using Markdown.Entities;
using static Markdown.Inlines.InlineSyntax;

namespace Markdown;

public class BlockSegmenter
{
    private const string CarriageReturnParagraphSeparator = "\r\n\r\n";
    private const string ParagraphSeparator = "\n\n";

    public IReadOnlyList<Block> Segment(string text)
    {
        var paragraphs = SplitToParagraphs(text);
        var blocks = new List<Block>();

        foreach (var paragraph in paragraphs)
        {
            var blockType = paragraph.StartsWith(Sharp) ? BlockType.Heading : BlockType.Paragraph;
            var rawText = paragraph.TrimStart('#', ' ');
            var block = new Block(rawText, blockType);
            blocks.Add(block);
        }

        return blocks;
    }

    private static string[] SplitToParagraphs(string text)
    {
        return text.Split([CarriageReturnParagraphSeparator, ParagraphSeparator], StringSplitOptions.RemoveEmptyEntries);
    }
}
