namespace Markdown
{
    public interface ITokenReader
    {
        TextToken TyrGetToken(string text, int end, int start);
    }
}