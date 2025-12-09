namespace MarkupConverter.Parsers.InlineParsers;

public class Delimiter
{
    public int NodeIndex;
    public int Length;
    public bool CanOpen;
    public bool CanClose;
    public bool IsInsideWord;
    public bool Matched;
}