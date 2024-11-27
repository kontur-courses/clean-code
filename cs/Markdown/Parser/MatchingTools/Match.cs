namespace Markdown.Parser.MatchingTools;

public class Match<T>(int start, int length, List<T> source)
{
    public int Start { get; } = start;
    public int Length { get; } = length;
    public int End { get; } = start + length;
    
    public static Match<T> ZeroMatch => new(-1, -1, []);

    public bool IsEmpty => Start == -1 && Length == -1;

    public List<T> ToList()
        => source
            .Skip(Start)
            .Take(Length)
            .ToList();
}