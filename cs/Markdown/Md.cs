using Markdown.Entities;
using Markdown.Inlines;

namespace Markdown;

public class Md(BlockSegmenter segmenter, InlineParser parser, HtmlRenderer renderer)
{
    public string Render(string text)
    {
        var segmentedBlocks = segmenter.Segment(text);
        var blocks = new List<Block>(segmentedBlocks.Count);

        foreach (var block in segmentedBlocks)
        {
            var inlines = parser.Parse(block.RawText);

            blocks.Add(new Block(block.RawText, block.Type)
            {
                Inlines = inlines
            });
        }

        var html = renderer.Render(blocks);

        return html;
    }
}
