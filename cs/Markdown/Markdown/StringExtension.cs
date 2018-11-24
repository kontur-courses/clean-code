using System.Text;


namespace Markdown
{
    public static class StringExtension
    {
        public static string ClearFromSymbol(this string str, char symbol)
        {
            if (str == null)
                return str;

            var builder = new StringBuilder();

            foreach (var character in str)
                if (character != symbol)
                    builder.Append(character);

            return builder.ToString();
        }
    }
}