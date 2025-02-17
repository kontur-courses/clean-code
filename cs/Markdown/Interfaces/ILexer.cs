using System.Collections.Immutable;

namespace Markdown.Interfaces;

public interface ILexer
{
    public IImmutableList<Token> Tokenize(string text);
}