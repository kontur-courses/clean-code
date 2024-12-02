using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokenizer.Tokens;

public class TokenList : IEnumerable<Token>
{
    private readonly List<Token> tokens;

    public TokenList(IEnumerable<Token> tokens)
    {
        this.tokens = tokens?.ToList() ?? throw new ArgumentNullException(nameof(tokens));
    }

    public IEnumerator<Token> GetEnumerator() => tokens.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool PeekOr(params TypeOfToken[][] choices)
    {
        return choices.Any(types => Peek(types));
    }

    public bool Peek(params TypeOfToken[] types)
    {
        if (types.Length > tokens.Count)
            return false;

        return !types.Where((type, i) => tokens[i].Type != type).Any();
    }

    public bool PeekAt(int index, params TypeOfToken[] types)
    {
        return Offset(index).Peek(types);
    }
    
    public bool PeekAtOr(int index, params TypeOfToken[][] choices)
    {
        return choices.Any(types => PeekAt(index, types));
    }

    public TokenList Offset(int index)
    {
        if (index == 0)
            return this;
        return index >= tokens.Count ? new TokenList(new List<Token>()) : new TokenList(tokens.Skip(index));
    }

    public override string ToString()
    {
        var tokenStrings = tokens.Select(t => t.ToString());
        return $"[\n\t{string.Join(",\n\t", tokenStrings)}\n]";
    }

    public Token this[int bodyConsumed] => tokens[bodyConsumed];
}