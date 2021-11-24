namespace Markdown
{
    public interface IParser
    {
        ParsingResult Parse(string mdText, int startBoundary, int endBoundary);
    }
}