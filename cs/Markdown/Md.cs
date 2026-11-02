namespace Markdown;

public static class Md
{
    private static int FindNextCharIndex(string markdown, char symbol, int startIndex)
    {
        // получение ближайшего справа индекса символа symbol
        throw new NotImplementedException();
    }

    private static bool IsSymbolMarkdownTag(string markdown, int startIndex)
    {
        // проверка является ли последовательность с startIndex мд тегом
        throw new NotImplementedException();
    }

    private static bool IsMarkdownTagOpening(string markdown, int startIndex)
    {
        throw new NotImplementedException();
    }
    
    private static bool IsMarkdownTagClosing(string markdown, int startIndex)
    {
        throw new NotImplementedException();
    }
    
    private static bool IsMarkdownTagScreened(string markdown, int startIndex)
    {
        throw new NotImplementedException();
    }
    
    public static string Render(string markdown)
    {
        return "";
    }
}