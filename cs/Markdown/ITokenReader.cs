namespace Markdown
{
    public interface ITokenReader
    {
        bool TryReadToken(string text, string context, int index, out IToken? token);
    }
}