namespace Markdown;

public abstract class TokenParser
{
    public abstract string StartPositionSymbol { get; }
    public abstract string EndPositionSymbol { get; }

    public abstract TokenType ParsingType { get; }

    protected abstract char? escapedSymbol { get; }

    protected abstract List<TokenParser> parsersForChild {  get; }

    public bool IsSymbolCanBeStartOfToken(char symbol) => symbol == StartPositionSymbol[0];

    public virtual ParseResult TryParseStringToToken(string value)
    {
        if (value.Length < StartPositionSymbol.Length + EndPositionSymbol.Length)
            return ParseResult.NullResult;
        var endPosition = FindEndPosition(value[StartPositionSymbol.Length..]);

        if (!IsStartAndEndPositionCorrect(value, 0, endPosition))
            return ParseResult.NullResult;

        var tokenText = value[StartPositionSymbol.Length..endPosition];
        if (!IsTokenTextCorrect(tokenText))
            return ParseResult.NullResult;
        var childs = FindChildsInTokenText(tokenText);
        return new(true, ConvertToToken(tokenText, childs), GetTokenLength(tokenText));
    }

    protected int GetTokenLength(string tokenText) => tokenText.Length + StartPositionSymbol.Length + EndPositionSymbol.Length;

    protected bool IsTokenTextCorrect(string text)
    {
        if (text.Length == 0) return false;
        if (text.All(char.IsDigit)) return false;
        return true;
    }

    protected bool IsStartAndEndPositionCorrect(string text, int start, int end)
    {
        if (start == -1 || end == -1)
            return false;

        var startSymbols = StartPositionSymbol.Length + start < text.Length
            ? text[start..(start + StartPositionSymbol.Length)]
            : string.Empty;
        var endSymbols = EndPositionSymbol.Length + end - 1 < text.Length
            ? text[end..(end + EndPositionSymbol.Length)]
            : string.Empty;

        if (startSymbols != StartPositionSymbol || endSymbols != EndPositionSymbol)
            return false;

        var isNextSymbolWhiteSpace = start + StartPositionSymbol.Length < text.Length
            && char.IsWhiteSpace(text[start + StartPositionSymbol.Length]);

        var isPrevSymbolWhiteSpace = end - EndPositionSymbol.Length > 0
            && char.IsWhiteSpace(text[end - 1]);
        if (isNextSymbolWhiteSpace || isPrevSymbolWhiteSpace)
            return false;
        if (IsTokenInAnotherWords(text, start, end))
            return false;
        return true;
    }

    protected bool IsTokenInAnotherWords(string text, int start, int end)
    {
        var isStartInWord = !(start < StartPositionSymbol.Length
            || char.IsWhiteSpace(text[start - StartPositionSymbol.Length]));
        var isEndInWord = !(end == text.Length - EndPositionSymbol.Length
            || char.IsWhiteSpace(text[end + EndPositionSymbol.Length])) && EndPositionSymbol != "\n";

        return (isEndInWord || isStartInWord) && text[start..end].Any(char.IsWhiteSpace);
    }

    protected int FindEndPosition(string text)
    {
        var endPosition = 0;
        do
        {
            endPosition = text.IndexOf(EndPositionSymbol, endPosition + 1);
            if (endPosition == -1)
                return endPosition;

            var nextSymbolIsUnderscore = !(endPosition == text.Length - EndPositionSymbol.Length
                                         || !(text[endPosition + EndPositionSymbol.Length] == escapedSymbol));
            var prevSymbolIsUnderscore = !(endPosition == 0
                                         || !(text[endPosition - 1] == escapedSymbol));

            if (!(nextSymbolIsUnderscore || prevSymbolIsUnderscore))
                return endPosition + StartPositionSymbol.Length;
        }
        while (true);
    }

    protected List<Token> FindChildsInTokenText(string text)
    {
        if (parsersForChild.Count == 0)
            return [];
        var parser = new MarkdownParser(parsersForChild);
        var tokens = parser.ParseTextToTokens(text);
        return tokens;
    }

    protected Token ConvertToToken(string value, List<Token> childs, string argument = null)
    {
        if (argument is not null)
            return new TokenWithArgument(ParsingType, value, argument);
        if (childs.All(t => t.Type == TokenType.SimpleText) || childs.Count == 0)
            return new SimpleToken(ParsingType, value);

        return new ComplexToken(ParsingType, childs);
    }
}
