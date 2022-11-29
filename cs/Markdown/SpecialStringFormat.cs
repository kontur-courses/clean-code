using System.Text;

namespace Markdown;

internal class SpecialStringFormat
{
    private static string EscapableCharacters = @"_\";

    private static readonly Dictionary<char, string> OperationalCharactersBackConverter =
        new()
        {
            { '_', "_" },
            { ';', "__" },
            { '#', "#" }
        };

    public string ConvertedLine { get; private set; }

    public bool[] OperationalCharacters { get; private set; }

    public MarkdownAction[] Actions { set; get; }

    public List<Tuple<int, int>> ActionPairs { get; set; }

    public static SpecialStringFormat getStringFormat(string originalLine)
    {
        var specialStringFormat = new SpecialStringFormat();
        HelperToRender helperToRender = new HelperToRender(originalLine);
        for (int i = 0; i < originalLine.Length; i++)
        {
            if (helperToRender.escapeSymbol)
            {
                if (EscapableCharacters.Contains(originalLine[i]))
                {
                    helperToRender.sb.Append(originalLine[i]);
                }
                else if (originalLine[i] == '#' && i == 1)
                {
                    helperToRender.sb.Append('#');
                }
                else
                {
                    helperToRender.sb.Append('\\');
                    helperToRender.sb.Append(originalLine[i]);
                }

                helperToRender.escapeSymbol = false;
            }
            else
            {
                helperToRender = CheckPrefix(helperToRender, originalLine, i);
            }
        }

        if (helperToRender.h1)
        {
            helperToRender.sb.Append('#');
            helperToRender.operationalCharacters[helperToRender.sb.Length - 1] = true;
        }

        specialStringFormat.ConvertedLine = helperToRender.sb.ToString();
        specialStringFormat.OperationalCharacters = helperToRender.operationalCharacters;
        return specialStringFormat;
    }

    private static HelperToRender CheckPrefix(HelperToRender helperToRender, string originalLine, int index)
    {
        HelperToRender newHelper = helperToRender;
        switch (originalLine[index])
        {
            case '\\':
                helperToRender.escapeSymbol = true;
                break;
            case '_':
                if (newHelper.sb.Length > 0 && newHelper.operationalCharacters[newHelper.sb.Length - 1] 
                                         && newHelper.sb[^1] == '_') newHelper.sb[^1] = ';';
                else
                {
                    newHelper.sb.Append('_');
                    helperToRender.operationalCharacters[newHelper.sb.Length - 1] = true;
                }

                break;
            case '#':
                newHelper.sb.Append('#');
                if (index == 0)
                {
                    helperToRender.h1 = true;
                    helperToRender.operationalCharacters[0] = true;
                }

                break;
            default:
                newHelper.sb.Append(originalLine[index]);
                break;
        }

        return newHelper;
    }
    public SpecialStringFormat()
    {
    }

    public SpecialStringFormat(SpecialStringFormat origin)
    {
        ConvertedLine = origin.ConvertedLine;
        OperationalCharacters = origin.OperationalCharacters;
        Actions = origin.Actions;
        ActionPairs = origin.ActionPairs;
    }

    public string ConvertFromFormat()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < ConvertedLine.Length; i++)
        {
            if (OperationalCharacters[i] && !Actions[i].Approved)
                sb.Append(OperationalCharactersBackConverter[ConvertedLine[i]]);
            else if (OperationalCharacters[i])
                sb.Append(
                    MarkdownTagToHTMLConverter.GetTagByMarkdownAction(Actions[i].ActionType, ConvertedLine[i]));
            else
                sb.Append(ConvertedLine[i]);
        }

        return sb.ToString();
    }
}