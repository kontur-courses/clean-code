namespace Markdown.Tags;

public class EscapeTag : ITag
{
    public string Head => "";
    private string symbol;
    public string Tail => symbol;
    public string MdView => """\""";

    public int InputStateNumber { get; } = 1;
    public int OutputStateNumber { get; } = 2;

    public Dictionary<int, Dictionary<SymbolStatus, int>> States { get; } = InitialzeStates();

    public SymbolStatus[] UsedSymbols { get; } =
    {
        SymbolStatus.backslash, SymbolStatus.underscore, SymbolStatus.anotherSymbol
    };

    public static Dictionary<int, Dictionary<SymbolStatus, int>> InitialzeStates()
    {
        var states = new Dictionary<int, Dictionary<SymbolStatus, int>>();

        states.Add(0, new Dictionary<SymbolStatus, int>());
        states.Add(1, new Dictionary<SymbolStatus, int>());
        states.Add(2, new Dictionary<SymbolStatus, int>());

        states[0].Add(SymbolStatus.backslash, 1);
        states[0].Add(SymbolStatus.underscore, 0);
        states[0].Add(SymbolStatus.anotherSymbol, 0);

        // Tag Open
        states[1].Add(SymbolStatus.backslash, 2);
        states[1].Add(SymbolStatus.underscore, 2);
        states[1].Add(SymbolStatus.anotherSymbol, 0);

        // Tag Close
        states[2].Add(SymbolStatus.backslash, 0);
        states[2].Add(SymbolStatus.underscore, 0);
        states[2].Add(SymbolStatus.anotherSymbol, 0);

        return states;
    }
}