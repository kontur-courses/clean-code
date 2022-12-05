namespace Markdown;

public class Md
{
    private readonly Parser _handler;

    public Md(Parser handler)
    {
        _handler = handler;
    }

    public string Render(string markdownText)
    {
        return _handler.Parse(markdownText);
    }
}