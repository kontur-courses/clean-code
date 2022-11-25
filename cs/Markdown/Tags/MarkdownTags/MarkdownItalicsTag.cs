using Markdown.Token;

namespace Markdown.Tags.MarkdownTags;

public record MarkdownItalicsTag() : Tag("_", "_")
{
    public override bool ClosesOnNewLine => false;
    public override bool CanIntersect => true;

    public override bool IsValidOpening(Stack<TokenTree> stack, string text, int position)
    {
        if (!IsOpeningSequence(text, position))
            return false;

        if (!TryGetNextChar(text, position, Opening, out var nextChar))
            return false;
        
        if (char.IsWhiteSpace(nextChar))
            return false;
        
        if (TryGetPreviousChar(text, position, Opening, out var previousChar))
            return !(char.IsDigit(previousChar) && char.IsDigit(nextChar));

        return true;
    }

    public override bool IsValidClosing(Stack<TokenTree> stack, string text, int position)
    {
        if (!IsClosingSequence(text, position)) 
            return false;

        if (!TryGetPreviousChar(text, position, Closing, out var previousChar))
            return false;

        if (char.IsWhiteSpace(previousChar))
            return false;
        
        if (TryGetNextChar(text, position, Closing, out var nextChar))
            return !(char.IsDigit(previousChar) && char.IsDigit(nextChar));

        return true;
    }
}