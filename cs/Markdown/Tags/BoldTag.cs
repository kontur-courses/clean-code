namespace Markdown.Tags;

public class BoldTag : ITag
{
    public string Head => "<strong>";
    public string Tail => "</strong>";
    public string MdView => "__";

    public int InputStateNumber { get; } = 2;
    public int OutputStateNumber { get; } = 11;

    public Dictionary<int, Dictionary<SymbolStatus, int>> States { get; } = InitialzeStates();

    public SymbolStatus[] UsedSymbols { get; } =
    {
        SymbolStatus.text, SymbolStatus.digit, SymbolStatus.underscore
        , SymbolStatus.eof, SymbolStatus.newline, SymbolStatus.space,
        SymbolStatus.anotherSymbol
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
        states.Add(6, new Dictionary<SymbolStatus, int>());
        states.Add(7, new Dictionary<SymbolStatus, int>());
        states.Add(8, new Dictionary<SymbolStatus, int>());
        states.Add(9, new Dictionary<SymbolStatus, int>());
        states.Add(10, new Dictionary<SymbolStatus, int>());
        states.Add(11, new Dictionary<SymbolStatus, int>());
        states.Add(12, new Dictionary<SymbolStatus, int>());
        states.Add(13, new Dictionary<SymbolStatus, int>());

        states[0].Add(SymbolStatus.text, 0);
        states[0].Add(SymbolStatus.digit, 0);
        states[0].Add(SymbolStatus.underscore, 1);
        states[0].Add(SymbolStatus.eof, 0);
        states[0].Add(SymbolStatus.newline, 0);
        states[0].Add(SymbolStatus.space, 0);
        states[0].Add(SymbolStatus.anotherSymbol, 0);

        states[1].Add(SymbolStatus.text, 9);
        states[1].Add(SymbolStatus.digit, 0);
        states[1].Add(SymbolStatus.underscore, 2);
        states[1].Add(SymbolStatus.eof, 0);
        states[1].Add(SymbolStatus.newline, 0);
        states[1].Add(SymbolStatus.space, 0);
        states[1].Add(SymbolStatus.anotherSymbol, 9);

        // Tag Open
        states[2].Add(SymbolStatus.text, 4);
        states[2].Add(SymbolStatus.digit, 0);
        states[2].Add(SymbolStatus.underscore, 3);
        states[2].Add(SymbolStatus.eof, 0);
        states[2].Add(SymbolStatus.newline, 0);
        states[2].Add(SymbolStatus.space, 5);
        states[2].Add(SymbolStatus.anotherSymbol, 4);

        states[3].Add(SymbolStatus.text, 0);
        states[3].Add(SymbolStatus.digit, 0);
        states[3].Add(SymbolStatus.underscore, 3);
        states[3].Add(SymbolStatus.eof, 0);
        states[3].Add(SymbolStatus.newline, 0);
        states[3].Add(SymbolStatus.space, 0);
        states[3].Add(SymbolStatus.anotherSymbol, 0);

        states[4].Add(SymbolStatus.text, 4);
        states[4].Add(SymbolStatus.digit, 0);
        states[4].Add(SymbolStatus.underscore, 7);
        states[4].Add(SymbolStatus.eof, 0);
        states[4].Add(SymbolStatus.newline, 0);
        states[4].Add(SymbolStatus.space, 5);
        states[4].Add(SymbolStatus.anotherSymbol, 4);

        states[5].Add(SymbolStatus.text, 6);
        states[5].Add(SymbolStatus.digit, 0);
        states[5].Add(SymbolStatus.underscore, 12);
        states[5].Add(SymbolStatus.eof, 0);
        states[5].Add(SymbolStatus.newline, 0);
        states[5].Add(SymbolStatus.space, 5);
        states[5].Add(SymbolStatus.anotherSymbol, 6);

        states[6].Add(SymbolStatus.text, 6);
        states[6].Add(SymbolStatus.digit, 0);
        states[6].Add(SymbolStatus.underscore, 7);
        states[6].Add(SymbolStatus.eof, 0);
        states[6].Add(SymbolStatus.newline, 0);
        states[6].Add(SymbolStatus.space, 5);
        states[6].Add(SymbolStatus.anotherSymbol, 6);

        states[7].Add(SymbolStatus.text, 10);
        states[7].Add(SymbolStatus.digit, 0);
        states[7].Add(SymbolStatus.underscore, 8);
        states[7].Add(SymbolStatus.eof, 0);
        states[7].Add(SymbolStatus.newline, 0);
        states[7].Add(SymbolStatus.space, 10);
        states[7].Add(SymbolStatus.anotherSymbol, 10);

        states[8].Add(SymbolStatus.text, 11);
        states[8].Add(SymbolStatus.digit, 11);
        states[8].Add(SymbolStatus.underscore, 2);
        states[8].Add(SymbolStatus.eof, 11);
        states[8].Add(SymbolStatus.newline, 11);
        states[8].Add(SymbolStatus.space, 11);
        states[8].Add(SymbolStatus.anotherSymbol, 11);

        // Maybe Bold in Italic
        states[9].Add(SymbolStatus.text, 0);
        states[9].Add(SymbolStatus.digit, 0);
        states[9].Add(SymbolStatus.underscore, 1);
        states[9].Add(SymbolStatus.eof, 0);
        states[9].Add(SymbolStatus.newline, 0);
        states[9].Add(SymbolStatus.space, 0);
        states[9].Add(SymbolStatus.anotherSymbol, 0);

        // Maybe Italic in Bold
        states[10].Add(SymbolStatus.text, 6);
        states[10].Add(SymbolStatus.digit, 0);
        states[10].Add(SymbolStatus.underscore, 7);
        states[10].Add(SymbolStatus.eof, 0);
        states[10].Add(SymbolStatus.newline, 0);
        states[10].Add(SymbolStatus.space, 5);
        states[10].Add(SymbolStatus.anotherSymbol, 6);

        // TagClose
        states[11].Add(SymbolStatus.text, 0);
        states[11].Add(SymbolStatus.digit, 0);
        states[11].Add(SymbolStatus.underscore, 0);
        states[11].Add(SymbolStatus.eof, 0);
        states[11].Add(SymbolStatus.newline, 0);
        states[11].Add(SymbolStatus.space, 0);
        states[11].Add(SymbolStatus.anotherSymbol, 0);

        // Maybe Italic in Bold
        states[12].Add(SymbolStatus.text, 4);
        states[12].Add(SymbolStatus.digit, 0);
        states[12].Add(SymbolStatus.underscore, 12); 
        states[12].Add(SymbolStatus.eof, 0);
        states[12].Add(SymbolStatus.newline, 0);
        states[12].Add(SymbolStatus.space, 13);
        states[12].Add(SymbolStatus.anotherSymbol, 4);

        states[13].Add(SymbolStatus.text, 6);
        states[13].Add(SymbolStatus.digit, 0);
        states[13].Add(SymbolStatus.underscore, 12);
        states[13].Add(SymbolStatus.eof, 0);
        states[13].Add(SymbolStatus.newline, 0);
        states[13].Add(SymbolStatus.space, 5);
        states[13].Add(SymbolStatus.anotherSymbol, 6);

        return states;
    }
}