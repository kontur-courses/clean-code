namespace Markdown.Tags;

public class ItalicTextTag : ITag
{
    public string Head => "<em>";
    public string Tail => "</em>";
    public string MdView => "_";

    public int InputStateNumber { get; } = 17;
    public int OutputStateNumber { get; } = 12;

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
        states.Add(14, new Dictionary<SymbolStatus, int>());
        states.Add(15, new Dictionary<SymbolStatus, int>());
        states.Add(16, new Dictionary<SymbolStatus, int>());
        states.Add(17, new Dictionary<SymbolStatus, int>());
        states.Add(18, new Dictionary<SymbolStatus, int>());

        states[0].Add(SymbolStatus.text, 13);
        states[0].Add(SymbolStatus.digit, 0);
        states[0].Add(SymbolStatus.underscore, 1);
        states[0].Add(SymbolStatus.eof, 0);
        states[0].Add(SymbolStatus.newline, 0);
        states[0].Add(SymbolStatus.space, 0);
        states[0].Add(SymbolStatus.anotherSymbol, 13);

        states[1].Add(SymbolStatus.text, 17);
        states[1].Add(SymbolStatus.digit, 0);
        states[1].Add(SymbolStatus.underscore, 9);
        states[1].Add(SymbolStatus.eof, 0);
        states[1].Add(SymbolStatus.newline, 0);
        states[1].Add(SymbolStatus.space, 0);
        states[1].Add(SymbolStatus.anotherSymbol, 17);

        states[2].Add(SymbolStatus.text, 2);
        states[2].Add(SymbolStatus.digit, 0);
        states[2].Add(SymbolStatus.underscore, 5);
        states[2].Add(SymbolStatus.eof, 0);
        states[2].Add(SymbolStatus.newline, 0);
        states[2].Add(SymbolStatus.space, 3);
        states[2].Add(SymbolStatus.anotherSymbol, 2);

        states[3].Add(SymbolStatus.text, 4);
        states[3].Add(SymbolStatus.digit, 0);
        states[3].Add(SymbolStatus.underscore, 8);
        states[3].Add(SymbolStatus.eof, 0);
        states[3].Add(SymbolStatus.newline, 0);
        states[3].Add(SymbolStatus.space, 3);
        states[3].Add(SymbolStatus.anotherSymbol, 4);

        states[4].Add(SymbolStatus.text, 4);
        states[4].Add(SymbolStatus.digit, 0);
        states[4].Add(SymbolStatus.underscore, 5);
        states[4].Add(SymbolStatus.eof, 0);
        states[4].Add(SymbolStatus.newline, 0);
        states[4].Add(SymbolStatus.space, 3);
        states[4].Add(SymbolStatus.anotherSymbol, 4);

        states[5].Add(SymbolStatus.text, 12);
        states[5].Add(SymbolStatus.digit, 12);
        states[5].Add(SymbolStatus.underscore, 6);
        states[5].Add(SymbolStatus.eof, 12);
        states[5].Add(SymbolStatus.newline, 12);
        states[5].Add(SymbolStatus.space, 12);
        states[5].Add(SymbolStatus.anotherSymbol, 12);

        states[6].Add(SymbolStatus.text, 7);
        states[6].Add(SymbolStatus.digit, 0);
        states[6].Add(SymbolStatus.underscore, 8);
        states[6].Add(SymbolStatus.eof, 0);
        states[6].Add(SymbolStatus.newline, 0);
        states[6].Add(SymbolStatus.space, 7);
        states[6].Add(SymbolStatus.anotherSymbol, 7);

        // Maybe Bold in Italic
        states[7].Add(SymbolStatus.text, 2);
        states[7].Add(SymbolStatus.digit, 0);
        states[7].Add(SymbolStatus.underscore, 5);
        states[7].Add(SymbolStatus.eof, 0);
        states[7].Add(SymbolStatus.newline, 0);
        states[7].Add(SymbolStatus.space, 3);
        states[7].Add(SymbolStatus.anotherSymbol, 2);

        states[8].Add(SymbolStatus.text, 2);
        states[8].Add(SymbolStatus.digit, 0);
        states[8].Add(SymbolStatus.underscore, 8);
        states[8].Add(SymbolStatus.eof, 0);
        states[8].Add(SymbolStatus.newline, 0);
        states[8].Add(SymbolStatus.space, 3);
        states[8].Add(SymbolStatus.anotherSymbol, 2);

        states[9].Add(SymbolStatus.text, 10);
        states[9].Add(SymbolStatus.digit, 0);
        states[9].Add(SymbolStatus.underscore, 11);
        states[9].Add(SymbolStatus.eof, 0);
        states[9].Add(SymbolStatus.newline, 0);
        states[9].Add(SymbolStatus.space, 0);
        states[9].Add(SymbolStatus.anotherSymbol, 10);

        // Maybe Italic in Bold
        states[10].Add(SymbolStatus.text, 0);
        states[10].Add(SymbolStatus.digit, 0);
        states[10].Add(SymbolStatus.underscore, 1);
        states[10].Add(SymbolStatus.eof, 0);
        states[10].Add(SymbolStatus.newline, 0);
        states[10].Add(SymbolStatus.space, 0);
        states[10].Add(SymbolStatus.anotherSymbol, 0);

        states[11].Add(SymbolStatus.text, 0);
        states[11].Add(SymbolStatus.digit, 0);
        states[11].Add(SymbolStatus.underscore, 11);
        states[11].Add(SymbolStatus.eof, 0);
        states[11].Add(SymbolStatus.newline, 0);
        states[11].Add(SymbolStatus.space, 0);
        states[11].Add(SymbolStatus.anotherSymbol, 11);

        // Tag Close
        states[12].Add(SymbolStatus.text, 0);
        states[12].Add(SymbolStatus.digit, 0);
        states[12].Add(SymbolStatus.underscore, 0);
        states[12].Add(SymbolStatus.eof, 0);
        states[12].Add(SymbolStatus.newline, 0);
        states[12].Add(SymbolStatus.space, 0);
        states[12].Add(SymbolStatus.anotherSymbol, 0);

        states[13].Add(SymbolStatus.text, 13);
        states[13].Add(SymbolStatus.digit, 0);
        states[13].Add(SymbolStatus.underscore, 14);
        states[13].Add(SymbolStatus.eof, 0);
        states[13].Add(SymbolStatus.newline, 0);
        states[13].Add(SymbolStatus.space, 0);
        states[13].Add(SymbolStatus.anotherSymbol, 13);

        states[14].Add(SymbolStatus.text, 18);
        states[14].Add(SymbolStatus.digit, 0);
        states[14].Add(SymbolStatus.underscore, 9);
        states[14].Add(SymbolStatus.eof, 0);
        states[14].Add(SymbolStatus.newline, 0);
        states[14].Add(SymbolStatus.space, 0);
        states[14].Add(SymbolStatus.anotherSymbol, 18);

        states[15].Add(SymbolStatus.text, 15);
        states[15].Add(SymbolStatus.digit, 0);
        states[15].Add(SymbolStatus.underscore, 16);
        states[15].Add(SymbolStatus.eof, 0);
        states[15].Add(SymbolStatus.newline, 0);
        states[15].Add(SymbolStatus.space, 0);
        states[15].Add(SymbolStatus.anotherSymbol, 15);

        states[16].Add(SymbolStatus.text, 12);
        states[16].Add(SymbolStatus.digit, 12);
        states[16].Add(SymbolStatus.underscore, 14);
        states[16].Add(SymbolStatus.eof, 12);
        states[16].Add(SymbolStatus.newline, 12);
        states[16].Add(SymbolStatus.space, 12);
        states[16].Add(SymbolStatus.anotherSymbol, 12);

        // Tag Open
        states[17].Add(SymbolStatus.text, 2);
        states[17].Add(SymbolStatus.digit, 0);
        states[17].Add(SymbolStatus.underscore, 5);
        states[17].Add(SymbolStatus.eof, 0);
        states[17].Add(SymbolStatus.newline, 0);
        states[17].Add(SymbolStatus.space, 3);
        states[17].Add(SymbolStatus.anotherSymbol, 2);

        // Tag Open
        states[18].Add(SymbolStatus.text, 15);
        states[18].Add(SymbolStatus.digit, 0);
        states[18].Add(SymbolStatus.underscore, 16);
        states[18].Add(SymbolStatus.eof, 0);
        states[18].Add(SymbolStatus.newline, 0);
        states[18].Add(SymbolStatus.space, 0);
        states[18].Add(SymbolStatus.anotherSymbol, 15);

        return states;
    }
}