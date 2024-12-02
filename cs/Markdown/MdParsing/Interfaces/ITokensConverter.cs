using Markdown.Tokens;

namespace Markdown.MdParsing.Interfaces;

public interface ITokensConverter
{
    public string Convert(IReadOnlyList<Token> tokens);
}