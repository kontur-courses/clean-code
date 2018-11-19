namespace Markdown
{
    public interface IReadable
    {
        Token tryGetToken(ref string input, int startPos);
    }
}