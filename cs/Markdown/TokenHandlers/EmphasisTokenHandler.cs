using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public class EmphasisTokenHandler : BoundaryTokenHandler
{
    protected override string Delimiter => "_";
    protected override TokenType TokenType => TokenType.Emphasis;

    public override bool CanHandle(char current, char next, MarkdownParseContext context) 
        => current == '_' && next != '_';
}