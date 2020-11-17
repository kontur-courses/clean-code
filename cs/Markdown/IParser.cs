namespace Markdown
{
    public interface IParser
    {
        public TagInfo Parse(string text);
    }
}