namespace Markdown;
public class Md
{
    private readonly TokenWrapper tokenWrapper;
    private readonly string text;

    public Md(string text, WrapperSettingsProvider settings)
    {
        this.text = text;
        tokenWrapper = new TokenWrapper(settings);
    }

    public string Render()
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<Token> ParseText(string text)
    {
        throw new NotImplementedException();
    }
}
