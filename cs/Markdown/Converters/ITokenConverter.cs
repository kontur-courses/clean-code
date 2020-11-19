using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface ITokenConverter
    {
        string ConvertToken(IToken token);
    }
}