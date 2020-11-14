namespace Markdown
{
    public interface IParser
    {
        public TextInfo Parse(string text);
    }
}