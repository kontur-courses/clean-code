namespace Markdown.Tags;

public interface ITag
{
    public string MdView { get; }
    public string Head { get;}
    public string Tail { get;}

    public SymbolStatus[] UsedSymbols { get; }
    public Dictionary<int, Dictionary<SymbolStatus, int>> States { get; }

    public int InputStateNumber { get; }
    public int OutputStateNumber { get; }
}