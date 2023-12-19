namespace Markdown.TagClasses;

public class WindowsNewLineTag : NewLineTag
{
    public override string MarkdownOpening => "\r\n";
}