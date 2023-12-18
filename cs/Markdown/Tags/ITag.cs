namespace Markdown.Tags;

public interface ITag
{
    public string Md { get; }
    public string Html { get; }
}