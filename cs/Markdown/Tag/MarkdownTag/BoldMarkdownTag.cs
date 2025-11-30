namespace Markdown;

public class BoldMarkdownTag() : MarkdownTag(TagType.Bold, "__", true)
{
    public new static readonly string TagText = "__";

    public override bool IsStartOfTag(string text, int position, bool isSameTagAlreadyOpen)
    {
        if (TagText.Length + position > text.Length) return false;

        var isSatisfiesConditions = false;
        if (isSameTagAlreadyOpen)
            isSatisfiesConditions = !char.IsWhiteSpace(text[position - 1]);
        else if (text.Length > position + TagText.Length)
            isSatisfiesConditions = !char.IsWhiteSpace(text[position + TagText.Length]);
        else
            isSatisfiesConditions = true;

        return text.AsSpan(position, TagText.Length).Equals(TagText, StringComparison.Ordinal) && isSatisfiesConditions;
    }

    public override bool IsTagCorrect(string text, int endPosition, OpenToken openToken)
    {
        if (openToken.OpenTag.TagType != TagType.Bold)
            throw new ArgumentException($"{openToken.OpenTag.TagType.ToString()} should be {nameof(TagType.Bold)}");

        var isTokenEmpty = openToken.TextStartPosition == endPosition && openToken.NestedTokens.Count == 0;

        return !IsTokenLocatedInsideWords(text, endPosition, openToken) &&
               !IsTokenHighlightsPartOfWordWithDigits(text, endPosition, openToken) && !isTokenEmpty;
    }

    private bool IsTokenLocatedInsideWords(string text, int endPosition, OpenToken openToken)
    {
        var tagLength = TagText.Length;
        var symbolBeforeStartTag = openToken.TextStartPosition - tagLength - 1;

        var isStartTagInMiddleWord = symbolBeforeStartTag >= 0 &&
                                     !char.IsWhiteSpace(text[openToken.TextStartPosition]) &&
                                     !char.IsWhiteSpace(text[symbolBeforeStartTag]);
        var isEndTagInMiddleWord = endPosition + tagLength < text.Length &&
                                   !char.IsWhiteSpace(text[endPosition + tagLength]) &&
                                   !char.IsWhiteSpace(text[endPosition - 1]);
        var textContainsSeveralWords = false;

        var part = text.AsSpan(openToken.TextStartPosition, endPosition - openToken.TextStartPosition);

        foreach (var symbol in part)
            if (char.IsWhiteSpace(symbol))
            {
                textContainsSeveralWords = true;
                break;
            }

        return (isStartTagInMiddleWord || isEndTagInMiddleWord) && textContainsSeveralWords;
    }

    private bool IsTokenHighlightsPartOfWordWithDigits(string text, int endPosition, OpenToken openToken)
    {
        var tagLength = TagText.Length;
        var symbolBeforeStartTag = openToken.TextStartPosition - tagLength - 1;
        var isStartTagInMiddleWord = symbolBeforeStartTag >= 0 &&
                                     !char.IsWhiteSpace(text[openToken.TextStartPosition]) &&
                                     !char.IsWhiteSpace(text[symbolBeforeStartTag]);
        var isEndTagInMiddleWord = endPosition + tagLength < text.Length &&
                                   !char.IsWhiteSpace(text[endPosition + tagLength]) &&
                                   !char.IsWhiteSpace(text[endPosition - 1]);

        var isOnlyPartOfWordHighlighted = isStartTagInMiddleWord || isEndTagInMiddleWord;
        var partText = text.Substring(openToken.TextStartPosition, endPosition - openToken.TextStartPosition);

        return isOnlyPartOfWordHighlighted && partText.All(char.IsLetterOrDigit) && partText.Any(char.IsDigit);
    }
}