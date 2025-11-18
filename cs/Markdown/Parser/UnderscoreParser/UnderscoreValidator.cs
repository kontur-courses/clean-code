using System.Text;

namespace Markdown;

public class UnderscoreValidator(TokenIndexer indexer)
{
    public bool CanOpen(int markerIndex)
    {
        return indexer.GetTokenAt(markerIndex + 1).Type != TokenType.Whitespace;
    }

    public bool CanClose(int markerIndex)
    {
        return indexer.GetTokenAt(markerIndex - 1).Type != TokenType.Whitespace;
    }

    public bool HasValidContent(int openIndex, int closeIndex)
    {
        if (!HasContent(openIndex, closeIndex))
            return false;

        return ContentNotOnlyDigits(openIndex, closeIndex) &&
               IsValidWordSpan(openIndex, closeIndex);
    }

    public bool HasClosingUnderscoreAhead(Underscore underscoreToMatch, int fromTokenIndex)
    {
        var targetType = underscoreToMatch.IsStrong ? TokenType.DoubleUnderscore : TokenType.Underscore;

        for (var i = fromTokenIndex + 1; i < indexer.Tokens.Count; i++)
        {
            if (indexer.Tokens[i].Type != targetType)
                continue;

            if (CanClose(i))
                return true;
        }

        return false;
    }

    private bool HasContent(int openIndex, int closeIndex)
    {
        for (var i = openIndex + 1; i < closeIndex; i++)
        {
            var value = indexer.Tokens[i].Value;
            if (!string.IsNullOrEmpty(value))
                return true;
        }

        return false;
    }

    private bool ContentNotOnlyDigits(int openIndex, int closeIndex)
    {
        var builder = new StringBuilder();

        for (var i = openIndex + 1; i < closeIndex; i++)
        {
            var value = indexer.Tokens[i].Value;
            if (!string.IsNullOrEmpty(value))
                builder.Append(value);
        }

        var text = builder.ToString();
        var trimmed = new string(text.Where(c => !char.IsWhiteSpace(c)).ToArray());
        if (trimmed.Length == 0)
            return false;

        return !trimmed.All(char.IsDigit);
    }

    private bool IsValidWordSpan(int openIndex, int closeIndex)
    {
        var openingInsideWord = IsUnderscoreInsideWord(openIndex);
        var closingInsideWord = IsUnderscoreInsideWord(closeIndex);

        if (!openingInsideWord && !closingInsideWord)
            return true;

        for (var i = openIndex + 1; i < closeIndex; i++)
        {
            var value = indexer.Tokens[i].Value;
            if (string.IsNullOrEmpty(value))
                continue;

            if (value.Any(char.IsWhiteSpace))
                return false;
        }

        return true;
    }

    private bool IsUnderscoreInsideWord(int index)
    {
        var before = indexer.GetTokenAt(index - 1);
        var after = indexer.GetTokenAt(index + 1);

        return before.Type == TokenType.Text && after.Type == TokenType.Text;
    }
}
