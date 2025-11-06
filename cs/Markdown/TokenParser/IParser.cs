using Markdown.Node;

namespace Markdown.TokenParser;

public interface IParser
{
    public INode Parse(List<Token> tokens);
}