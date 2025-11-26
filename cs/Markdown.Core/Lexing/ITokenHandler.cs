namespace Markdown.Core.Lexing;
public interface ITokenHandler
{
    bool CanHandle(ReadOnlySpan<char> source, int index);
    int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens);
}
