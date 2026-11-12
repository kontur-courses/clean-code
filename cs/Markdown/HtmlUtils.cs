namespace Markdown;

public static class HtmlUtils
{
    // возможно, лучше делать не статический класс, чтобы например можно было 
    // задавать словарь во время работы программы,
    // но на данном этапе мне кажется, что это нормально
    private static readonly Dictionary<string, string> mdToHtmlMap;

    static HtmlUtils()
    {
        mdToHtmlMap = new Dictionary<string, string>
        {
            { "_", "em" },
            { "__", "strong" },
            { "#", "h1" },
            
        };
    }

    public static string ConvertMdTagToHtml(string markdownTag, bool isOpening)
    {
        if (!mdToHtmlMap.ContainsKey(markdownTag))
            throw new ArgumentException("Неизвестный Markdown-тег.",
                nameof(markdownTag));
        return isOpening ? $"<{mdToHtmlMap[markdownTag]}>" : $"</{mdToHtmlMap[markdownTag]}>";
    }
}