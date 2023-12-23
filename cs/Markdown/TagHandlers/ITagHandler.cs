namespace Markdown.TagHandlers;

public interface ITagHandler
{
    public KeyValuePair<string, string> MdHtmlTagPair { get; }

    public bool CanTransform(string text);

    public StringManipulator Transform(string text);
}