namespace Markdown;

public class Md
{
    public string Render(string expression)
    {
        var lexer = new Lexer(expression);
        var tokens = lexer.GetTokens().ToArray();
        var parser = new Parser(tokens);
        var evaluator = new HTMLEvaluator();
        var tree = parser.Parse();
        var result = evaluator.Evaluate(tree);
        return result;
    }
}