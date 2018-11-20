using System.Linq;
using System.Text;

namespace Markdown.Analyzers
{
    public class EscapesAnalyzer
    {
        public static bool[] GetBitMaskOfEscapedChars(string markdown)
        {
            bool[] isEscapedCharAt = new bool[markdown.Length];
            for (var index = 0; index < markdown.Length; index++)
            {
                if (index == 0 || markdown[index - 1] != '\\')
                    isEscapedCharAt[index] = false;
                else
                    isEscapedCharAt[index] = !isEscapedCharAt[index - 1];
            }

            return isEscapedCharAt;
        }

        public static string RemoveEscapeSlashes(string markdown, char[] escapeChars)
        {
            var result = new StringBuilder();
            var isEscapedCharAt = GetBitMaskOfEscapedChars(markdown);

            for (var index = 0; index < markdown.Length - 1; index++)
                if (!(isEscapedCharAt[index + 1] && escapeChars.Contains(markdown[index + 1])))
                    result.Append(markdown[index]);
            result.Append(markdown[markdown.Length - 1]);

            return result.ToString();
        }
    }
}
