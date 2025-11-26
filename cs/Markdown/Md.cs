namespace Markdown;

public class Md
{
    private Tokenizer tokenizer = new Tokenizer();
    private Parser parser = new Parser();
    private Renderer renderer = new Renderer();
    public string Render(string markdown)
    {
        List<Token> tokens = tokenizer.TextToTokens(markdown);
        MainNode tree = parser.ParseTokensToTree(tokens);
        string resultHTML = renderer.RenderTreeToHTML(tree);
        return resultHTML;
    }
    
}

