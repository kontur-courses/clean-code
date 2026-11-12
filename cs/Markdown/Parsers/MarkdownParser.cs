using System.Text;
using Markdown.Entities;
using Markdown.Interfaces;

namespace Markdown.Parsers;

public sealed class MarkdownParser(IReadOnlyList<IBlockTag> blockTags)
{
    public IReadOnlyList<MarkdownBlock> Parse(string markdownText)
    {
        if (string.IsNullOrEmpty(markdownText))
        {
            return [];
        }

        var normalized = markdownText.Replace("\r\n", "\n").Replace('\r', '\n');
        var blocks = new List<MarkdownBlock>();
        foreach (var blockText in SplitIntoBlocks(normalized))
        {
            blocks.Add(ParseBlock(blockText));
        }

        return blocks;
    }

    private MarkdownBlock ParseBlock(string markdownBlockText)
    {
        var tag = blockTags.First(candidate => candidate.CanHandle(markdownBlockText));
        return new MarkdownBlock
        {
            Tag = tag,
            Content = markdownBlockText
        };
    }

    private static List<string> SplitIntoBlocks(string markdownText)
    {
        var blocks = new List<string>();
        var current = new StringBuilder();
        var hasContent = false;
        foreach (var line in markdownText.Split('\n'))
        {
            if (line.Length == 0)
            {
                if (!hasContent)
                {
                    continue;
                }

                blocks.Add(current.ToString());
                current.Clear();
                hasContent = false;

                continue;
            }

            if (hasContent)
            {
                current.Append('\n');
            }

            current.Append(line);
            hasContent = true;
        }

        if (hasContent)
        {
            blocks.Add(current.ToString());
        }

        return blocks;
    }
}