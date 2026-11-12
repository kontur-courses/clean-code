using Markdown.Nodes;

namespace Markdown.Parsing.Parsers;

public interface IParser
{
    List<INode> Parse(List<INode> nodes);
}