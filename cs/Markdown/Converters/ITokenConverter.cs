using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface ITokenConverter
    {
        string Convert(Token token);
    }
}