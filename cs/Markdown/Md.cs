using System.Text;
using Markdown.Parsing;

namespace Markdown;

public class Md
{
    private readonly BlockParser _parser;

    public Md()
    {
        var inlineParser = new InlineParser();
        _parser = new BlockParser(inlineParser);
    }

    public string Render(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var nodes = _parser.Parse(text);
        var sb = new StringBuilder();

        foreach (var node in nodes)
        {
            node.Render(sb);
        }

        return sb.ToString();
    }
}