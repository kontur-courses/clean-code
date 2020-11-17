namespace Markdown
{
    public interface ITokenReader
    {
        Token? TryReadToken(string text, int index);
    }
}