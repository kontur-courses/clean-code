namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    internal interface IMarkdownCollectorDependent
    {
        void SetCollector(MarkdownCollector collector);
    }
}