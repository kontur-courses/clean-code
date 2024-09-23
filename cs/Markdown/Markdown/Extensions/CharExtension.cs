namespace Markdown.Extensions;

public static class CharExtension
{
    public static char EmptyChar => '\0';

    public static bool IsEmpty(this char ch)
    {
        return ch == EmptyChar;
    }
}