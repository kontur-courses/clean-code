namespace Markdown;

public class TokenWrappers
{
    public string TokenStart { get; set; }
    public string TokenEnd { get; set; }

    public TokenWrappers(string start, string end)
    {
        TokenStart = start;
        TokenEnd = end;
    }
}