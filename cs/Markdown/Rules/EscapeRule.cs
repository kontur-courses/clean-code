using Markdown.Tags;

namespace Markdown.Rules;

public class EscapeRule : IRule
{
    private readonly ITag tag = new EscapeTag();
    private List<Token.Token> tokens = new();
    private int currentState;

    public TagKind MoveByRule(char ch, int position)
    {
        var symbol = SymbolStatusParser.ParseSymbolStatus(ch);

        if (!tag.UsedSymbols.Contains(symbol))
        {
            symbol = SymbolStatus.anotherSymbol;
        }

        currentState = tag.States[currentState][symbol];

        if (currentState == tag.InputStateNumber)
        {
            tokens.Add(new Token.Token(tag.MdView, tag.Head, position - (tag.MdView.Length - 1)));
            return TagKind.Open;
        }
        else if (currentState == tag.OutputStateNumber)
        {
            tokens.Add(new Token.Token(ch.ToString(), ch.ToString(), position - (tag.MdView.Length - 1)));
            currentState = 0;
            return TagKind.Close;
        }

        return TagKind.None;
    }

    public List<Token.Token> GetTokens() { return tokens; }

    public void ClearTokens() { tokens.Clear(); }
}