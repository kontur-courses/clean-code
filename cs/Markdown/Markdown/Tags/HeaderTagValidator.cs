namespace Markdown.Markdown.Tags;

public class HeaderTagValidator : ITagValidator
{
    private const char StartTagContent = '#'; 
    private const char EndTagContent = ' '; 

    public bool IsValidTag(string content) =>
        content is [StartTagContent, EndTagContent];

    public bool IsTagStart(string content) =>
        content.StartsWith(StartTagContent);

    public bool IsTagEnd(string content) =>
        content.EndsWith(EndTagContent);
}