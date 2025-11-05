using Markdown.Nodes.Interfaces;

namespace Markdown.Parsing.Interfaces;

public interface IParser
{
    bool CanParse(string text);
    MarkdownNode Parse(string text);
}