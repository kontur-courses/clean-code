namespace Markdown.Blocks;

public class LineSplitter
{
    public IReadOnlyList<string> Split(string markdown)
    {
        return markdown.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
    }
}