namespace Markdown;

public class ItalicTokenParser : TokenParser
{
    public override string StartPositionSymbol => "_";
    public override string EndPositionSymbol => "_";

    public override TokenType ParsingType => TokenType.Italic;

    protected override List<TokenParser> parsersForChild => [];

    protected override char? escapedSymbol => '_';
}
