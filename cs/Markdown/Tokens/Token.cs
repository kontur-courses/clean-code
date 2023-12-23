namespace Markdown.Tokens;

public abstract class Token
{
    protected abstract string TagWrapper { get; }
    protected abstract string Separator { get; }
    protected abstract bool IsCanContainAnotherTags { get; }
    protected abstract bool IsSingleSeparator { get; }
    public int OpeningIndex { get; private protected set; }
    public int ClosingIndex { get; private protected set; }
    public IEnumerable<Token> Tokens { get; }

    protected Token(int openingIndex, int closingIndex)
    {
        if (openingIndex < 0 || closingIndex <= openingIndex)
        {
            throw new ArgumentException();
        }

        OpeningIndex = openingIndex;
        ClosingIndex = closingIndex;
        Tokens = new List<Token>();
    }

    protected Token(int openingIndex)
    {
        if (openingIndex < 0)
        {
            throw new ArgumentException();
        }

        OpeningIndex = openingIndex;
        Tokens = new List<Token>();
    }
    
    public void CloseToken(int closingIndex)
    {
        if (closingIndex <= OpeningIndex)
        {
            throw new ArgumentException();
        }

        ClosingIndex = closingIndex;
    }

    public bool IsTokenSingleSeparator()
    {
        return IsSingleSeparator;
    }

    public string GetSeparator()
    {
        return Separator;
    }
    public virtual string GetTokenContent()
    {
        return $"<{Separator}>{String.Join(string.Empty,Tokens)}</{Separator}>";
    }
}