using System.Collections;

namespace Markdown.Tokens;

public abstract class Token
{
    public abstract string TagWrapper { get; }
    public abstract string Separator { get; }
    public abstract bool IsCanContainAnotherTags { get; }
    public abstract bool IsSingleSeparator { get; }
    public abstract bool IsContented { get; }
    public int OpeningIndex { get; }
    public int ClosingIndex { get; private set; }
    public List<Token> Tokens { get; set; } = new();

    public bool IsCorrect { get; set; }
    public bool IsClosed { get; set; }


    protected Token(int openingIndex, int closingIndex)
    {
        if (openingIndex < 0 || closingIndex < openingIndex)
        {
            throw new ArgumentException();
        }

        IsClosed = true;
        OpeningIndex = openingIndex;
        ClosingIndex = closingIndex;
    }

    protected Token(int openingIndex)
    {
        if (openingIndex < 0)
        {
            throw new ArgumentException();
        }

        OpeningIndex = openingIndex;
    }

    public void CloseToken(int closingIndex)
    {
        if (closingIndex <= OpeningIndex)
        {
            throw new ArgumentException();
        }

        ClosingIndex = closingIndex;
        IsClosed = true;
    }

    public string GetSeparator()
    {
        return Separator;
    }

    public abstract void Validate(string str, IEnumerable<Token> tokens);

    public virtual bool CanCloseToken(int closeIndex, string str)
    {
        return true;
    }

    public override string ToString()
    {
        if (IsCorrect)
            return $"<{TagWrapper}>{String.Join(string.Empty, Tokens)}</{TagWrapper}>";

        return $"{Separator}" +
               $"{String.Join(string.Empty, Tokens)}" +
               $"{(IsSingleSeparator ? string.Empty : Separator)}";
    }
}