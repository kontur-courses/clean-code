namespace Markdown;

public class MdTextToken : IToken
{
    public string GetValue { get; }

    public MdTextToken(string text)
    {
        GetValue = text;
    }
}