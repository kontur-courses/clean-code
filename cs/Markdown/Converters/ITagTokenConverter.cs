using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface ITagTokenConverter
    {
        string ConvertToken(IToken token);
    }
}