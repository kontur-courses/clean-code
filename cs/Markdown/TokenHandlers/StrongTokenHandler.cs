using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public class StrongTokenHandler : BoundaryTokenHandler
{
    protected override string Delimiter => "__";
    protected override TokenType TokenType => TokenType.Strong;

    public override bool CanHandle(char current, char next, MarkdownParseContext context) 
        => current == '_' && next == '_';
}