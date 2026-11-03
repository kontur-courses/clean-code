namespace Markdown;
 
/// <summary>
/// Проставляет Token.Role разделителям за один линейный проход
/// читает и меняет Token, для LinkToken рекурсивно разбирает Content
/// </summary>

internal sealed class EmphasisResolver
{
    private readonly string text;
    private readonly List<Token> tokens;
    private readonly int[] whitespacesBefore;

    private EmphasisResolver(string text, List<Token> tokens)
    {
        this.text = text;
        this.tokens = tokens;
        whitespacesBefore = CountWhitespaces(text);
    }

    public static void Resolve(string text, List<Token> tokens)
        => new EmphasisResolver(text, tokens).Run();

    private void Run()
    {

    }


    private static int[] CountWhitespaces(string text)
    {
        throw new NotImplementedException();
    }
}