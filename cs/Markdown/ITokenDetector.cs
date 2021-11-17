namespace Markdown
{
    public interface ITokenDetector
    {
        Token GetNextToken(int from, string text);
    }
}