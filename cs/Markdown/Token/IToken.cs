namespace Markdown.Token;

public interface IToken
{
    int Position { get; }
    int EndingPosition { get; }
    int Length { get; }
    string Separator { get; }
    bool IsPair { get; }
    bool IsClosed { get; set; }
    bool IsParametrized { get; }
    string Parameters { get; set; }
    int Shift { get; set; }
    bool IsValid(string source, ref List<IToken> tokens, IToken currentToken);
}