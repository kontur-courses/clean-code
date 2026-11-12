using Markdown.Syntax;

namespace Markdown.Blocks;

public class BlockParser
{
    public IReadOnlyList<Block> Parse(IReadOnlyList<string> lines)
    {
        var blocks = new List<Block>();
        var currentBlockLines = new List<string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                FlushBlock(blocks, currentBlockLines);
                continue;
            }

            currentBlockLines.Add(line);
        }

        FlushBlock(blocks, currentBlockLines);
        return blocks;
    }

    private static void FlushBlock(List<Block> blocks, List<string> lines)
    {
        if (lines.Count == 0)
            return;

        var firstLine = lines[0];
        var isHeader = firstLine.StartsWith(MarkupSymbols.HeaderMarker, StringComparison.Ordinal);

        var normalizedFirstLine = firstLine;
        if (isHeader)
            normalizedFirstLine = firstLine[MarkupSymbols.HeaderMarker.Length..];
        else if (firstLine.StartsWith(MarkupSymbols.EscapedHeaderMarker, StringComparison.Ordinal))
            normalizedFirstLine = firstLine[MarkupSymbols.EscapeLength..];

        var content = lines.Count == 1
            ? normalizedFirstLine
            : string.Join(LineBreaks.NewLineSequence, PrependFirstLine(normalizedFirstLine, lines));

        var kind = isHeader ? BlockKind.Header : BlockKind.Paragraph;
        blocks.Add(new Block(kind, content));
        lines.Clear();
    }

    private static IEnumerable<string> PrependFirstLine(string firstLine, List<string> remainingLines)
    {
        yield return firstLine;
        for (var i = 1; i < remainingLines.Count; i++)
            yield return remainingLines[i];
    }
}