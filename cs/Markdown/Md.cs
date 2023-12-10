namespace Markdown;

public class Md
{
    private readonly ISplitter splitter;
    private readonly IWrapper wrapper;

    public Md(ISplitter splitter, IWrapper wrapper)
    {
        this.splitter = splitter;
        this.wrapper = wrapper;
    }

    public string Render(string markdownText)
    {
        throw new NotImplementedException();
    }
}