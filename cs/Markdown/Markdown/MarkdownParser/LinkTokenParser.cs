namespace Markdown;

public class LinkTokenParser : TokenParser
{
    public override string StartPositionSymbol => "[";

    public override string EndPositionSymbol => ")";

    public override TokenType ParsingType => TokenType.Link;

    protected override char? escapedSymbol => ')';

    protected override List<TokenParser> parsersForChild => [];

    public override ParseResult TryParseStringToToken(string value)
    {
        if (value.Length < StartPositionSymbol.Length + EndPositionSymbol.Length)
            return ParseResult.NullResult;
        var endPosition = FindEndPosition(value[StartPositionSymbol.Length..]);

        if (!IsStartAndEndPositionCorrect(value, 0, endPosition))
            return ParseResult.NullResult;

        if (!IsTokenTextCorrect(value))
            return ParseResult.NullResult;

        var textAndArg = FindTextAndArgumentOfLink(value);
        if (textAndArg == null)
            return ParseResult.NullResult;
        return new(
            true,
            ConvertToToken(textAndArg.Value.Item1, [], textAndArg.Value.Item2),
            GetTokenLength(value[StartPositionSymbol.Length..endPosition]));
    }

    public (string, string)? FindTextAndArgumentOfLink(string value)
    {
        var endText = value.IndexOf(']');
        var startArgument = value.IndexOf('(');
        var endArgument = value.IndexOf(')');

        if (!IsChildsPositionsCorrect(endText, startArgument, endArgument))
            return null;

        var text = value[1..endText];
        var arg = value[(startArgument + 1)..endArgument];
        if (text.Contains('[') || arg.Contains('('))
            return null;

        return (text, arg);     
    }

    private bool IsChildsPositionsCorrect(int endText, int startArg, int endArg)
    {
        return !(endText == -1 || startArg == -1 || endArg == -1)
            && (endText + 1 == startArg);
    }
}
