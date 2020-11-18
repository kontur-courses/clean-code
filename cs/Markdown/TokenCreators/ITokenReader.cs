namespace Markdown
{
    public interface ITokenReader
    {
        TextToken TyrGetToken(string text, int index, int startPosition);
    }
}