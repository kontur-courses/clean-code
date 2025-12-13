using Markdown.Nodes.Interfaces;

namespace Markdown.Parsers.Interfaces;

public interface IParser
{
    public ParseStatus TryParse(out MarkdownNode node);
}