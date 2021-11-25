namespace Markdown
{
    public interface IParser
    {
        ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary);
    }
}