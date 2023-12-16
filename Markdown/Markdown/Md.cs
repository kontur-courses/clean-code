using System.Diagnostics;

namespace Markdown;

public class Md
{
    public string Render(string source)
    {
        var tags = new List<ITag>();
        return new MarkdownBuilder(source, tags).Build();
    }
}