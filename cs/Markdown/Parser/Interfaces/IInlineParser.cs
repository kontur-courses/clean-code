using Markdown.Node.Interfaces;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Interfaces;

public interface IInlineParser
{
    IReadOnlyList<INode> Parse(IReadOnlyList<Token> tokens);
}
