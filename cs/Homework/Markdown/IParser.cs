namespace Markdown
{
    public interface IParser
    {
        public Token[] Parse(string text);
    }
}