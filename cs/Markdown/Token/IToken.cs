namespace Markdown.Token;

public interface IToken
{
    int Position { get; }
    int Length { get; }
    string Separator { get; }
    bool IsPair { get; }
    bool IsClosed { get; set; }
    bool IsParametrized { get; }
    List<string> Parameters { get; }
    int TokenSymbolsShift { get; }
    bool IsValid(string source, List<IToken> tokens, IToken currentToken);
}