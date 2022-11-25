using System.Text;

namespace Markdown
{
    internal class SpecialStringFormat
    {
        public string ConvertedLine { get; private set; }

        public bool[] OperationalCharacters { get; private set; }

        public MarkdownAction[] Actions { get; set; }

        public List<Tuple<int, int>> ActionPairs { get; set; }

        private SpecialStringFormat()
        {
        }

        private static string EscapableCharacters = @"_\";

        public static SpecialStringFormat ConvertLineToFormat(string originalLine)
        {
            var sb = new StringBuilder();
            bool escapeSymbol = false;
            bool h1 = false;
            var operationalCharacters = new bool[originalLine.Length + 1];

            for (int i = 0; i < originalLine.Length; i++)
            {
                if (escapeSymbol)
                {
                    if (EscapableCharacters.Contains(originalLine[i]))
                    {
                        sb.Append(originalLine[i]);
                    }
                    else if (originalLine[i] == '#' && i == 1)
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('\\');
                        sb.Append(originalLine[i]);
                    }

                    escapeSymbol = false;
                }
                else
                {
                    switch (originalLine[i])
                    {
                        case '\\':
                            escapeSymbol = true;
                            break;
                        case '_':
                            if (sb.Length > 0 && operationalCharacters[sb.Length - 1] && sb[^1] == '_') sb[^1] = ';';
                            else
                            {
                                sb.Append('_');
                                operationalCharacters[sb.Length - 1] = true;
                            }

                            break;
                        case '#':
                            sb.Append('#');
                            if (i == 0)
                            {
                                h1 = true;
                                operationalCharacters[0] = true;
                            }

                            break;
                        default:
                            sb.Append(originalLine[i]);
                            break;
                    }
                }
            }

            if (h1)
            {
                sb.Append('#');
                operationalCharacters[sb.Length - 1] = true;
            }


            var sFormat = new SpecialStringFormat();
            sFormat.ConvertedLine = sb.ToString();
            sFormat.OperationalCharacters = operationalCharacters;
            return sFormat;
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

        private static readonly Dictionary<char, string> OperationalCharactersBackConverter =
            new()
            {
                {'_', "_"},
                {';', "__"},
                {'#', "#"}
            };
    }
}