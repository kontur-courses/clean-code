using Markdown.Tokens;

namespace Markdown.TokenConverter;

public interface ITokenConverter
{
    string ConvertToString(char tokenSeparator, List<Token> tokens);
}