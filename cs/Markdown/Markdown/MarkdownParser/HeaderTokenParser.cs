namespace Markdown;

public class HeaderTokenParser : TokenParser
{
    public override string StartPositionSymbol => "# ";

    public override string EndPositionSymbol => "\n";

    public override TokenType ParsingType => TokenType.Header;

    protected override List<TokenParser> parsersForChild => 
        [new ItalicTokenParser(), new BoldTokenParser()];

    protected override char? escapedSymbol => null;
}
