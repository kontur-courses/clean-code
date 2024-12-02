using Markdown.Tags;

namespace Markdown.Rules;

public class BoldRule : IRule
{
    private readonly ITag tag = new BoldTag();
    private List<Token.Token> tokens = new();
    private bool isItalicInStart;
    private bool isItalicInMiddle;
    private bool isTagClosed;
    private readonly int[] italicStartsInBoldState = [10, 12];
    private readonly int boldStartsInItalicState = 9;
    private readonly int fakeItalicStartState = 13;
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

        if (currentState == tag.InputStateNumber)
        {
            tokens.Add(new Token.Token(tag.MdView, tag.Head, position - (tag.MdView.Length - 1)));
        }
        else if (currentState == tag.OutputStateNumber)
        {
            if (symbol == SymbolStatus.eof)
            {
                tokens.Add(new Token.Token(tag.MdView, tag.Tail, position - (tag.MdView.Length - 1)));
            }
            else
            {
                tokens.Add(new Token.Token(tag.MdView, tag.Tail, position - tag.MdView.Length));
            }

            isTagClosed = true;
        }
        else if (currentState == boldStartsInItalicState)
        {
            if (isItalicInMiddle)
            {
                isItalicInStart = false;
                isItalicInMiddle = false;
                isTagClosed = false;
                currentState = 0;
                tokens.Clear();
            }
            else
            {
                isItalicInStart = !isItalicInStart;
            }
        }
        else if (italicStartsInBoldState.Contains(currentState))
        {
            if (isItalicInStart)
            {
                isItalicInStart = false;
                isItalicInMiddle = false;
                isTagClosed = false;
                currentState = 0;
                tokens.Clear();
            }
            else
            {
                isItalicInMiddle = !isItalicInMiddle;
            }

        }
        else if (currentState == fakeItalicStartState)
        {
            isItalicInMiddle = !isItalicInMiddle;
        }

        if (isTagClosed && (!isItalicInMiddle && !isItalicInStart || symbol == SymbolStatus.eof))
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