namespace Markdown.Primitives;

public static class Characters
{
    public const char Underline = '_';
    public const char Sharp = '#';
    public const char NewLine = '\n';
    public const char Escape = '\\';
    public const char WhiteSpace = ' ';

    public static HashSet<char> Specials = new HashSet<char>
    {
        Underline,
        NewLine,
        Escape
    };
}