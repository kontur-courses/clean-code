namespace Markdown;

public class Md
{
    public static string Render(string markdown)
    {
        TokenGenerator tokenGenerator = new TokenGenerator();
        Converter converter = new Converter();
        return converter.ConvertWithTokens(tokenGenerator.SplitParagraphs(markdown));
    }
}