namespace Markdown
{
    public interface IReadable
    {
        Token tryGetToken(string input, int startPos);
    }
}