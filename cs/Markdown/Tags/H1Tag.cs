namespace Markdown.Tags;

public class H1Tag : ITag
{
    public string Head => "<h1>";
    public string Tail => "</h1>";
    public string MdView => "# ";

    public int InputStateNumber { get; } = 3;
    public int OutputStateNumber { get; } = 5;

    public Dictionary<int, Dictionary<SymbolStatus, int>> States { get; } = InitialzeStates();

    public SymbolStatus[] UsedSymbols { get; } =
    {
        SymbolStatus.eof, SymbolStatus.newline, SymbolStatus.space,
        SymbolStatus.sharp, SymbolStatus.anotherSymbol
    };

    public static Dictionary<int, Dictionary<SymbolStatus, int>> InitialzeStates()
    {
        var states = new Dictionary<int, Dictionary<SymbolStatus, int>>();

        states.Add(0, new Dictionary<SymbolStatus, int>());
        states.Add(1, new Dictionary<SymbolStatus, int>());
        states.Add(2, new Dictionary<SymbolStatus, int>());
        states.Add(3, new Dictionary<SymbolStatus, int>());
        states.Add(4, new Dictionary<SymbolStatus, int>());
        states.Add(5, new Dictionary<SymbolStatus, int>());

        states[0].Add(SymbolStatus.sharp, 1);
        states[0].Add(SymbolStatus.space, 2);
        states[0].Add(SymbolStatus.newline, 0);
        states[0].Add(SymbolStatus.eof, 0);
        states[0].Add(SymbolStatus.anotherSymbol, 2);

        states[1].Add(SymbolStatus.sharp, 0);
        states[1].Add(SymbolStatus.space, 3);
        states[1].Add(SymbolStatus.newline, 0);
        states[1].Add(SymbolStatus.eof, 0);
        states[1].Add(SymbolStatus.anotherSymbol, 0);

        states[2].Add(SymbolStatus.sharp, 2);
        states[2].Add(SymbolStatus.space, 2);
        states[2].Add(SymbolStatus.newline, 0);
        states[2].Add(SymbolStatus.eof, 0);
        states[2].Add(SymbolStatus.anotherSymbol, 2);

        // Tag Open
        states[3].Add(SymbolStatus.sharp, 4);
        states[3].Add(SymbolStatus.space, 4);
        states[3].Add(SymbolStatus.newline, 5);
        states[3].Add(SymbolStatus.eof, 5);
        states[3].Add(SymbolStatus.anotherSymbol, 4);

        states[4].Add(SymbolStatus.sharp, 4);
        states[4].Add(SymbolStatus.space, 4);
        states[4].Add(SymbolStatus.newline, 5);
        states[4].Add(SymbolStatus.eof, 5);
        states[4].Add(SymbolStatus.anotherSymbol, 4);

        // Tag Close
        states[5].Add(SymbolStatus.sharp, 0);
        states[5].Add(SymbolStatus.space, 0);
        states[5].Add(SymbolStatus.newline, 0);
        states[5].Add(SymbolStatus.eof, 0);
        states[5].Add(SymbolStatus.anotherSymbol, 0);

        return states;
    }
}