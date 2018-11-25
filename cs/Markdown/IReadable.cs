namespace Markdown
{
    public interface IReadable
    {
        Token TryGetToken(string input, int startPos);
    }
}