namespace Markdown;

public class Md
{
    private readonly TokenGenerator tokenGenerator;
    private readonly NodeGenerator nodeGenerator;
    private readonly HtmlRenderer htmlRenderer;

    public Md()
    {
        tokenGenerator = new TokenGenerator();
        nodeGenerator = new NodeGenerator();
        htmlRenderer = new HtmlRenderer();
    }

    public string Render(string markdownText)
    {
        if (string.IsNullOrEmpty(markdownText))
            return string.Empty;

        var tokens = tokenGenerator.Tokenize(markdownText);
        var nods = nodeGenerator.Create(tokens);
        var html = htmlRenderer.Render(nods);

        return html;
    }
}