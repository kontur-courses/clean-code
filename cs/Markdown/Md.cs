namespace Markdown;

public class Md
{
    public string Render(string expression)
    {
        var lexer = new Lexer(expression);
        var tokens = lexer.GetTokens().ToArray();
        var parser = new Parser(tokens);
        var eval = new HTMLEvaluator();
        var tree = parser.Parse();
        var result = eval.Evaluate(tree);
        return result;
    }
}