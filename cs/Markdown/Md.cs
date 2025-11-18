using Markdown.Tags;
using Markdown.TokenHandlers;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    private readonly IReadOnlyList<ITag> _supportedTags;
    private readonly MarkdownParser _parser;

    public Md(List<ITag> supportedTags)
    {
        _supportedTags = supportedTags;
        _parser = new MarkdownParser(
        [
            new EscapeTokenHandler(), 
            new NewlineTokenHandler(),
            new TagTokenHandler()
        ],  new NestingHandler());
    }

    public string Render(string sourceText)
    {
        var lexer = new Lexer(sourceText, _supportedTags);
        var tokens = lexer.Tokenize();
        
        var parsedTokens = _parser.Parse(tokens);

        return parsedTokens.ConvertToHtml();
    }
}