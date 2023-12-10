namespace Markdown;

public class Tag
{
    public string markdownOpening { get; private set; }
    public string markdownClosing { get; private set; }
    public string htmlKeyword { get; private set; }
    public string tagOpen => throw new NotImplementedException(); // <htmlKeyword>
    public string tagClose => throw new NotImplementedException(); // </htmlKeyword>

    public Tag(string markdownOpening, string markdownClosing, string htmlKeyword)
    {
        this.markdownOpening = markdownOpening;
        this.markdownClosing = markdownClosing;
        this.htmlKeyword = htmlKeyword;
    }
}