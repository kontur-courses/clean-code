using System.Text;

namespace Markdown
{
    public class StringFormatter
    {
        private const string EscapableCharacters = @"_\";

        private static readonly Dictionary<char, string> ReversibleCharacters =
            new()
            {
                { '_', "_" },
                { ';', "__" },
                { '#', "#" }
            };

        public string FormattedString { get; private set; }
        public bool[] SpecialCharacters { get; private set; }
        public TagAction[] Actions { set; get; }
        public List<Tuple<int, int>> ActionPairs { get; set; }

        public static StringFormatter GetStringFormat(string originalString)
        {
            var stringFormatter = new StringFormatter();
            var helper = new StringConversionHelper(originalString);

            for (var i = 0; i < originalString.Length; i++)
            {
                if (helper.EscapingChar)
                {
                    if (EscapableCharacters.Contains(originalString[i]))
                    {
                        helper.Sb.Append(originalString[i]);
                    }
                    else if (originalString[i] == '#' && i == 1)
                    {
                        helper.Sb.Append('#');
                    }
                    else
                    {
                        helper.Sb.Append('\\');
                        helper.Sb.Append(originalString[i]);
                    }

                    helper.EscapingChar = false;
                }
                else
                {
                    helper = CheckPrefix(helper, originalString, i);
                }
            }

            if (helper.H1)
            {
                helper.Sb.Append('#');
                helper.OperationalCharacters[helper.Sb.Length - 1] = true;
            }

            stringFormatter.FormattedString = helper.Sb.ToString();
            stringFormatter.SpecialCharacters = helper.OperationalCharacters;
            return stringFormatter;
        }

        private static StringConversionHelper CheckPrefix(StringConversionHelper helper, string originalString, int index)
        {
            switch (originalString[index])
            {
                case '#':
                    helper.Sb.Append('#');
                    if (index == 0)
                    {
                        helper.H1 = true;
                        helper.OperationalCharacters[0] = true;
                    }

                    break;
                
                case '_':
                    if (helper.Sb.Length > 0 && helper.OperationalCharacters[helper.Sb.Length - 1] 
                                             && helper.Sb[^1] == '_') 
                        helper.Sb[^1] = ';';
                    else
                    {
                        helper.Sb.Append('_');
                        helper.OperationalCharacters[helper.Sb.Length - 1] = true;
                    }

                    break;
                
                case '\\':
                    helper.EscapingChar = true;
                    break;
                
                default:
                    helper.Sb.Append(originalString[index]);
                    break;
            }
            
            return helper;
        }

        private StringFormatter() { }

        public StringFormatter(StringFormatter origin)
        {
            FormattedString = origin.FormattedString;
            SpecialCharacters = origin.SpecialCharacters;
            Actions = origin.Actions;
            ActionPairs = origin.ActionPairs;
        }

        public string ConvertToFormat()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < FormattedString.Length; i++)
            {
                if (SpecialCharacters[i] && !Actions[i].IsValid)
                    sb.Append(ReversibleCharacters[FormattedString[i]]);
                else if (SpecialCharacters[i])
                    sb.Append(
                        TagToHtml.GetTagAction(Actions[i].ActionType, FormattedString[i]));
                else
                    sb.Append(FormattedString[i]);
            }

            return sb.ToString();
        }
    }
}
