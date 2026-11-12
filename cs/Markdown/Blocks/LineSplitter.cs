using Markdown.Syntax;

namespace Markdown.Blocks;

public class LineSplitter
{
    public IReadOnlyList<string> Split(string markdown)
    {
        return markdown
            .Replace(LineBreaks.WindowsSequence, LineBreaks.NewLineSequence)
            .Replace(LineBreaks.CarriageReturn, LineBreaks.NewLine)
            .Split(LineBreaks.NewLine);
    }
}