namespace Markdown
{
    public interface IParser
    {
        IToken TryGetToken();
    }
}
