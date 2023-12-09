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
    
    public string Render(string text)
    {
        throw new NotImplementedException();
    }

    private string RenderLine(string line)
    {
        throw new NotImplementedException();
    }
}