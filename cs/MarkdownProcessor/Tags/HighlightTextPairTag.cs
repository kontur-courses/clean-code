namespace MarkdownProcessor.Tags;

public abstract class HighlightTextPairTag : Tag
{
    private bool corrupted;
    private bool processedWhiteSpace;

    protected HighlightTextPairTag(Token openingToken) : base(openingToken)
    {
    }

    protected override bool StopRunToken(Token token)
    {
        if (string.IsNullOrWhiteSpace(token.Value)) processedWhiteSpace = true;

        return corrupted;
    }

    protected override bool StopRunTag(Tag tag)
    {
        return corrupted || ForbiddenChild(tag);
    }

    protected abstract bool ForbiddenChild(Tag tag);

    protected override bool IsClosingToken(Token token)
    {
        var isClosingToken = token.Value == Config.ClosingSign &&
                             !token.BeforeIsSpace &&
                             !token.BetweenDigits &&
                             !BlankStringBetweenToken(OpeningToken, token) &&
                             !(processedWhiteSpace && HighlightWordPart(OpeningToken, token));

        if (isClosingToken && Children.Any(t => !t.Closed))
            corrupted = true;

        return isClosingToken && !corrupted;
    }

    private static bool BlankStringBetweenToken(Token opening, Token closing)
    {
        return opening.TagFirstCharIndex + opening.Value.Length == closing.TagFirstCharIndex;
    }

    private static bool HighlightWordPart(Token opening, Token closing)
    {
        return !string.IsNullOrWhiteSpace(opening.Before.ToString()) ||
               !string.IsNullOrWhiteSpace(closing.After.ToString());
    }
}