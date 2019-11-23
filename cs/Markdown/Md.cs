using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;
            var tokens = new List<Token>();
            var position = 0;
            var tokenReader = new TokenReader();
            while (position < input.Length)
            {
                var (token, end) = tokenReader.GetToken(input, position);
                tokens.Add(token);
                position = end;
            }

            var htmlConverter = new TokenToHtmlConverter();
            var stringHTML = new StringBuilder();
            foreach (var token in tokens)
                stringHTML.Append(htmlConverter.ConvertToHtml(token));

            return stringHTML.ToString();
        }
    }
}