namespace Markdown.Tokens;

public class ItalicsToken : Token
{
    public override string TagWrapper => "em";
    public override bool IsTag => true;
    
    public ItalicsToken(string content) : base(content) {}
    
    public override bool Validate(List<Token> tokens, int index)
    {
        return !IsNearNumber(tokens, index) && (IsValid(tokens, index) || CanBeInWord(tokens, index));
    }

    public override bool IsOpen(List<Token> tokens, int index)
    {
        return CanBeOpen(tokens, index) || (!IsInClosingPosition(tokens, index) && CanBeInWord(tokens, index));
    }

    private static bool IsValid(List<Token> tokens, int index)
    {
        return CanBeClose(tokens, index) != CanBeOpen(tokens, index);
    }

    private static bool CanBeClose(List<Token> tokens, int index)
    {
        return index - 1 > 0 && tokens[index - 1] is not SpaceToken &&
               (index + 1 >= tokens.Count || tokens[index + 1].IsTag || tokens[index + 1] is SpaceToken);
    }
    
    private static bool CanBeOpen(List<Token> tokens, int index)
    {
        return index + 1 < tokens.Count && tokens[index + 1] is not SpaceToken &&
               (index - 1 < 0 || tokens[index - 1].IsTag || tokens[index - 1] is SpaceToken);
    }
    
    private static bool IsNearNumber(List<Token> tokens, int index)
    {
        return index - 1 >= 0 && index + 1 < tokens.Count &&
               ((tokens[index - 1] is LiteralToken && ((LiteralToken)tokens[index - 1]).IsNumber() &&
                 tokens[index + 1] is not SpaceToken) ||
                (tokens[index + 1] is LiteralToken && ((LiteralToken)tokens[index + 1]).IsNumber() &&
                 tokens[index - 1] is not SpaceToken));
    }
    
    private static bool CanBeInWord(List<Token> tokens, int index)
    {
        var neededToken = tokens[index].GetType();
        var amountInWord = 0;
        while (index - 1 >= 0 && tokens[index - 1] is not SpaceToken)
            index--;
        while (index < tokens.Count && tokens[index] is not SpaceToken)
        {
            if (tokens[index].GetType() == neededToken) 
                amountInWord++;
            index++;
        }
        return amountInWord % 2 == 0;
    }
    
    private static bool IsInClosingPosition(List<Token> tokens, int index)
    {
        var currentIndex = index - 1;
        var countBefore = 1;
        while (currentIndex >= 0 && tokens[currentIndex] is not SpaceToken)
        {
            if (tokens[currentIndex].GetType() == tokens[index].GetType())
                countBefore++;
            currentIndex--;
        }
        return countBefore % 2 == 0;
    }
}