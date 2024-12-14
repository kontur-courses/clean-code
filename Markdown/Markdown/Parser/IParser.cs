using Markdown.ParseTree;

namespace Markdown.Parser;

public interface IParser<TTokenType, in TToken>
{
    public IParseTree<TTokenType> Parse(IEnumerable<TToken> tokens);
}