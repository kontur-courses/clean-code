namespace Markdown.Tokens;

public abstract class Token
{
    protected abstract string TagWrapper { get; }
    protected abstract string Separator { get; }
    protected abstract bool IsCanContainAnotherTags { get; }
    protected abstract bool IsSingleSeparator {get;}
    public int OpeningIndex;
    public int ClosingIndex;
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
}