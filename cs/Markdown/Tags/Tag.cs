using Markdown.Token;

namespace Markdown.Tags;

public abstract record Tag(string Opening, string Closing)
{
    public abstract bool ClosesOnNewLine { get; }
    public abstract bool CanIntersect { get; }
    public abstract bool IsValidOpening(Stack<TokenTree> stack, string text, int position);
    public abstract bool IsValidClosing(Stack<TokenTree> stack, string text, int position);

    public virtual bool IsOpeningSequence(string text, int position)
    {
        if (position + Opening.Length - 1 >= text.Length) return false;

        return !Opening.Where((t, i) => text[position + i] != t).Any();
    }
    
    public virtual bool IsClosingSequence(string text, int position)
    {
        if (position + Closing.Length - 1 >= text.Length) return false;

        return !Closing.Where((t, i) => text[position + i] != t).Any();
    }

    protected bool TryGetNextChar(string text, int position, string sequence, out char result)
    {
        result = '\0';
        if (position + sequence.Length >= text.Length) return false;

        result = text[position + sequence.Length];
        return true;
    }
    
    protected bool TryGetPreviousChar(string text, int position, string sequence, out char result)
    {
        result = '\0';
        if (position - 1 < 0) return false;

        result = text[position - 1];
        return true;
    }
}