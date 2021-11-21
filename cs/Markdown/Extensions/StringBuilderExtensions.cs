using System.Linq;
using System.Text;

namespace Markdown.Extensions
{
    public static class StringBuilderExtensions
    {
        public static bool IsSingleWhiteSpace(this StringBuilder builder)
        {
            return builder.ToString() == " ";
        }

        public static bool HasDigits(this StringBuilder builder)
        {
            return builder.ToString().ToCharArray().Any(ch => char.IsDigit(ch));
        }
    }
}
