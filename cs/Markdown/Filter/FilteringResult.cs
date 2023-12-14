namespace Markdown.Filter;

public class FilteringResult
{
    public IReadOnlySet<int> EscapedPositions { get; }
    
    public string ResultingString { get; }
    

    public FilteringResult(IReadOnlySet<int> escapedPositions, string resultingString)
    {
        EscapedPositions = escapedPositions;
        ResultingString = resultingString;
    }
}