namespace Markdown;

public class BoldTokenParser : TokenParser
{
    public override string StartPositionSymbol => "__";

    public override string EndPositionSymbol => "__";

    public override TokenType ParsingType => TokenType.Bold;

    protected override List<TokenParser> parsersForChild => [new ItalicTokenParser()];

    protected override char? escapedSymbol => '_';
}
