using Markdown.AstNodes;
using Markdown.Tokens;

namespace Markdown;

public interface IParser
{
    RootMarkdownNode Parse(List<Token> tokens);
}