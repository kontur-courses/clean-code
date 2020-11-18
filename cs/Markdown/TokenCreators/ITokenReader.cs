namespace Markdown
{
    public interface ITokenReader
    {
        TextToken GetToken(string text, int index, int startPosition);
    }
}