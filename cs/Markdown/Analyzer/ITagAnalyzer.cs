using Markdown.Tags;

namespace Markdown.Analyzer
{
    public interface ITagAnalyzer
    {
        (Tag, int) GetTagTypeWithIndex(string line, int index);
    }
}
