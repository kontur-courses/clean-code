namespace Markdown;

public abstract class TokenBase
{
    public TokenBase(TokenType type, TokenBase? parent = null, int startPosition = -1, int endPosition = -1)
    {
        TokenType = type;
        Parent = parent;
        StartPosition = startPosition;
        EndPosition = endPosition;
    }
    
    public TokenBase? Parent { get; set; }
    
    public List<TokenBase> NestedTokens { get; set; }
    
    public TokenType TokenType { get; set; }
    
    public int StartPosition { get; set; }
    
    public bool IsStartPositionSet => StartPosition >= 0;
    
    public int EndPosition  { get; set; }
    
    public bool IsEndPositionSet => EndPosition >= 0;

    public virtual string ToText() =>  "";
    
    public virtual bool CanStartsHere(string text, int start) => false;

    public virtual string ToHtmlWithText(string text) => "";
    
    protected static bool IsAtSide(IReadOnlyList<TokenBase> tokens, int index, bool isAtLeftSide)
    {
        return false;
    }

    protected static bool HasOnlyTextBeforeEnding(TokenBase tokenBase, int openingIndex, int endingIndex)
    {
        return true;
    }

    protected static int GetEndingIndex(TokenBase tokenBase, int openingIndex)
    {
        return -1;
    }

    protected static List<int> GetPositions(IReadOnlyList<TokenBase> tokens, TokenType type)
    {
        return new List<int>();
    }

    protected static void ToText(IReadOnlyList<TokenBase> tokens, int start, int end)
    {
    }

    protected static void ToText(IReadOnlyList<TokenBase> tokens, IEnumerable<int> positions)
    {
    }

    protected static bool HasDigits(IReadOnlyList<TokenBase> tokens, int start, int end)
    {
        return false;
    }

    private static bool HasDigits(string text)
    {
        return text.Any(char.IsDigit);
    }
}