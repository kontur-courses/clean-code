using Markdown.Token;

namespace Markdown.Tags.MarkdownTags;

public record MarkdownHeadingTag() : Tag("# ", "")
{
    public override bool ClosesOnNewLine => true;
    public override bool CanIntersect => false;

    public override bool IsValidOpening(Stack<TokenTree> stack, string text, int position)
    {
        if (TryGetPreviousChar(text, position, Opening, out _))
            return false;

        return IsOpeningSequence(text, position);
    }

    public override bool IsValidClosing(Stack<TokenTree> stack, string text, int position) => false;

    public override bool IsClosingSequence(string text, int position) => false;
}