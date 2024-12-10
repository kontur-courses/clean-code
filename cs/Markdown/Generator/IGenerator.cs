using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Generator;

public interface IGenerator
{
    public string Render(Node root, List<Token> tokens);
}