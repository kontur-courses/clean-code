namespace Markdown
{
    public interface IParser
    {
        void register(IReadable reader);
        Token[] tokenize(string str);
    }
}