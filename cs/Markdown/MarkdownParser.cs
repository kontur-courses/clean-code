namespace Markdown;

public class MarkdownParser : IParserFrom
{
    private readonly ILexer lexer = new MarkdownLexer();

    public IEnumerable<MarkupToken> Parse(string markdownText)
    {
        throw new NotImplementedException();
    }
}