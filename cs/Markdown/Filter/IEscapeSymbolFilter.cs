namespace Markdown.Filter;

public interface IEscapeSymbolFilter
{
    public FilteringResult FilterEscapeSymbols(string initial);
}