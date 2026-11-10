using Markdown.Tokenization.Tokens.Abstractions;
using System.Collections;

namespace Markdown.Tokenization.Tokens;

public class UnresolvedTokens : IEnumerable<Token>
{
    private readonly IEnumerable<Token> _tokens;

    public UnresolvedTokens(IEnumerable<Token> tokens)
    {
        _tokens = tokens;
    }

    public IEnumerator<Token> GetEnumerator()
        => _tokens.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
