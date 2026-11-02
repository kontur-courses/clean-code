namespace Markdown;

public class Md : StringFormatter
{
    public readonly string Text;
    private readonly StringFormatter _stringFormatter;
    private readonly Dictionary<string, string> _pairReplacements = new()
    {
        { "_", "<em>" },
        { "__", "<strong>" }
    };
    private readonly Dictionary<string, string> _singularReplacements = new()
    {
        { "#", "<h1>" }
    };

    // Будет использовать StringFormatter
    public string Render()
    {

    }
}
