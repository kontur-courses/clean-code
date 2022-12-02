namespace Markdown;

public static class MarkdownEmTag
{
    public static string OpenTag => "_";
    public static string CloseTag => "_";
}

public static class MarkdownStrongTag
{
    public static string OpenTag => "__";
    public static string CloseTag => "__";
}

public static class MarkdownHeaderTag
{
    public static string OpenTag => "#";
    public static string CloseTag => "\n";
}