using Markdown.Lexer;
using Markdown.TokenConverter;

namespace Markdown.Renderer;

public class MarkdownRenderer : IMarkdownRenderer
{
    private readonly ILexer lexer;
    private readonly ITokenConverter tokenConverter;

    public MarkdownRenderer(ILexer lexer, ITokenConverter tokenConverter)
    {
        this.lexer = lexer;
        this.tokenConverter = tokenConverter;
    }

    //TODO: добавить cross-line рендер для тегов, работающих в разных строках (<ul>)
    public string Render(string text)
    {
        if (text is null)
            throw new ArgumentException("Input parameter cannot be null.");

        var lines = text.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);

        var renderedLines = lines
            .Select(RenderLine)
            .ToList();

        return string.Join('\n', renderedLines);
    }

    private string RenderLine(string line)
        => tokenConverter.ConvertToString(lexer.Tokenize(line));
}