namespace Markdown.Core.Lexing;

public interface ILexer
{
    public IEnumerable<Token> Tokenize(ReadOnlyMemory<char> source);
}