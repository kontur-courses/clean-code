#nullable enable
namespace Markdown
{
    public interface ITokenReader
    {
        Token? TryReadToken(TextParser parser, string text, int index);
    }
}