using Markdown.Tags;

namespace Markdown.Rules;

public class ItalicRule : IRule
{
    private readonly ITag tag = new ItalicTextTag();
    private List<Token.Token> tokens = new();
    private bool isBoldInMiddle;
    private bool isBoldInStart;
    private bool isTagClosed;
    private readonly int anotherStartState = 18;
    private readonly int italicStartsInBoldState = 7;
    private readonly int boldStartsInItalicState = 10;
    private int currentState;

    public TagKind MoveByRule(char ch, int position)
    {

        var symbol = SymbolStatusParser.ParseSymbolStatus(ch);

        if (!tag.UsedSymbols.Contains(symbol))
        {
            symbol = SymbolStatus.anotherSymbol;
        }

        currentState = tag.States[currentState][symbol];

        if (currentState == 0)
        {
            ClearTokens();
        }

        if (currentState == tag.InputStateNumber || currentState == anotherStartState)
        {
            tokens.Add(new Token.Token(tag.MdView, tag.Head, position - tag.MdView.Length));
        }
        else if (currentState == tag.OutputStateNumber)
        {
            if (symbol == SymbolStatus.eof)
            {
                tokens.Add(new Token.Token(tag.MdView, tag.Tail, position));
            }
            else
            {
                tokens.Add(new Token.Token(tag.MdView, tag.Tail, position - tag.MdView.Length));
            }
            isTagClosed = true;
        }
        else if (currentState == boldStartsInItalicState)
        {
            if (isBoldInMiddle)
            {
                isBoldInMiddle = false;
                isBoldInStart = false;
                isTagClosed = false;
                currentState = 0;
                tokens.Clear();
            }
            else
            {
                isBoldInStart = !isBoldInStart;
            }
        }
        else if (currentState == italicStartsInBoldState)
        {
            if (isBoldInStart)
            {
                isBoldInMiddle = false;
                isBoldInStart = false;
                isTagClosed = false;
                currentState = 0;
                tokens.Clear();

            }
            else
            {
                isBoldInMiddle = !isBoldInMiddle;
            }
        }

        if (isTagClosed && (!isBoldInMiddle || symbol == SymbolStatus.eof))
        {
            isTagClosed = false;
            currentState = 0;
            return TagKind.Close;
        }

        return TagKind.None;
    }

    public List<Token.Token> GetTokens() { return tokens; }

    public void ClearTokens() { tokens.Clear(); }
}