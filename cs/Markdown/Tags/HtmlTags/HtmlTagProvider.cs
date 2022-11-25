namespace Markdown.Tags.HtmlTags;

public static class HtmlTagProvider
{
    public static readonly Tag Bold = new HtmlBoldTag();
    public static readonly Tag Italics = new HtmlItalicsTag();
    public static readonly Tag Heading = new HtmlHeadingTag();
}