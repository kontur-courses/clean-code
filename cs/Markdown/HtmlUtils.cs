namespace Markdown;

public static class HtmlUtils
{
    // возможно, лучше делать не статический класс, чтобы например можно было 
    // задавать словарь во время работы программы,
    // но на данном этапе мне кажется, что это нормально
    public static readonly Dictionary<string, string> MdToHtmlMap;

    static HtmlUtils()
    {
        MdToHtmlMap = new Dictionary<string, string>
        {
            { "_", "em" },
            { "__", "strong" },
            { "#", "h1" },
            
        };
    }

    public static string ReplaceWithHtmlTag(string markdown, int startIndex, bool closing)
    {
        // меняет в строке тег, первый символ которого находится на startIndex
        throw new NotImplementedException();
    }
}