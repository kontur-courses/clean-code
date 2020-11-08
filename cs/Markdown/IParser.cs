namespace Markdown
{
    public interface IParser
    {
        public TextInfo ParseText(string text);
    }
}