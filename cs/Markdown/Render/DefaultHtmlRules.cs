namespace Markdown.Render;

public static class DefaultHtmlReplaceRules
{
    public static List<IReplaceRule> CreateDefaultRules()
    {
        return new List<IReplaceRule>()
        {
            new TagReplaceRule("_", "<em>", "</em>"),
            new TagReplaceRule("__", "<strong>", "</strong>"),
        };
    }
}