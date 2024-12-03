using Markdown.TagInfo;

namespace Markdown.HtmlTools;

internal static class HtmlTagExtensions
{
    public static string GetHtmlOpenTag(this Tag tag) => tag switch
    {
        Tag.FirstLevelHeader => "<h1>",
        Tag.SecondLevelHeader => "<h2>",
        Tag.ThirdLevelHeader => "<h3>",
        Tag.FourthLevelHeader => "<h4>",
        Tag.FifthLevelHeader => "<h5>",
        Tag.SixthLevelHeader => "<h6>",
        Tag.Italic => "<em>",
        Tag.Strong => "<strong>",
        var _ => ""
    };
}