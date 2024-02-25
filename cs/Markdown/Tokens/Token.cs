using System.Collections;

namespace Markdown.Tokens;

public abstract class Token
{
    public abstract string TagWrapper { get; }
    public abstract string Separator { get; }
    public abstract bool IsCanContainAnotherTags { get; }
    public abstract bool IsSingleSeparator { get; }
    public abstract bool IsContented { get; }
    public int OpeningIndex { get; private protected set; }
    public int ClosingIndex { get; private protected set; }
    public List<Token> Tokens { get; set; } = new();

    public bool IsCorrect { get; set; } = false;
    public bool IsClosed { get; set; } = false;


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
    
    public static bool IsCorrectTokenOpenSeparator(int separatorStart,int separatorEnd, string str)
    {
        return separatorEnd < str.Length - 1 && str[separatorEnd + 1] != ' ' && !IsSeparatorInsideDigit(separatorStart,separatorEnd,str);
    }

    public static bool IsCorrectTokenCloseSeparator(int separatorStart,int separatorEnd, string str)
    {
        return separatorEnd != 0 && str[separatorStart - 1] != ' ' && !IsSeparatorInsideDigit(separatorStart,separatorEnd,str);
    }

    public static bool IsSeparatorInsideDigit(int separatorStart, int separatorEnd, string str)
    {
        var isLeftDigit = (separatorStart > 0 && char.IsDigit(str[separatorStart - 1]));
        var isRightDigit = (separatorEnd < str.Length - 1 && char.IsDigit(str[separatorEnd + 1]));
        return isLeftDigit || isRightDigit;
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