using System.Text;

namespace Markdown;

internal class SpecialStringFormat
{
    private static string EscapableCharacters = @"_\";
    private static readonly Dictionary<char, string> OperationalCharactersBackConverter =
        new()
        {
            {'_', "_"},
            {';', "__"},
            {'#', "#"}
        };
    public string ConvertedLine { get; private set; }

    public bool[] OperationalCharacters { get; private set; }

    public MarkdownAction[] Actions { set; get; }

    public List<Tuple<int, int>> ActionPairs { get; set; }
    public SpecialStringFormat(string originalLine)
    {
    }
    public string ConvertFromFormat()
    {
        return "";
    }
}