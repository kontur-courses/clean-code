namespace Markdown;

public class Html
{
    private readonly Dictionary<TextStyleType, string> headers = new()
    {
        { TextStyleType.Bold, "<strong>"},
        { TextStyleType.Italic, "<em>"},
        { TextStyleType.Header, "<h1>"}
    };
    
    public string Create(string[] words, Specification specification)
    {
        throw new NotImplementedException();
    }
}