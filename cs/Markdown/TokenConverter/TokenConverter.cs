using Markdown.Tokens;

namespace Markdown.TokenConverter;

public class TokenConverter : ITokenConverter
{
    public string ConvertToString(char tokenSeparator, List<Token> tokens, IReadOnlySet<int> symbolsPosToRemove)
    {
        throw new NotImplementedException();
    }
}