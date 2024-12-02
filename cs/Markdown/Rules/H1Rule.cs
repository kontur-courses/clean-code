using Markdown.Tags;

namespace Markdown.Rules;

public class H1Rule : IRule
{
    private readonly ITag tag = new H1Tag();
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
        }
        else if (currentState == tag.OutputStateNumber)
        {
            if (symbol == SymbolStatus.eof)
            {
                tokens.Add(new Token.Token("", tag.Tail, position + 1));
            }
            else
            {
                tokens.Add(new Token.Token("", tag.Tail, position));
            }

            currentState = 0;
            return TagKind.Close;
        }

        return TagKind.None;
    }

    public List<Token.Token> GetTokens() { return tokens; }

    public void ClearTokens() { tokens.Clear(); }
}