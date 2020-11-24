using Markdown.Tokens;

namespace Markdown.Readers
{
    public interface ITokenReader
    {
        bool TryReadToken(string text, string context, int index, out Token? token);
    }
}