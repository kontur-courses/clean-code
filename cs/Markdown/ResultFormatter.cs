using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class ResultFormatter
    {
        public static string Form(IEnumerable<Token.Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                result
                    .Append(token.Element.OpenTag)
                    .Append(token.Content)
                    .Append(token.Element.ClosingTag);
            }

            return result.ToString();
        }

        public static string RemoveSlashes(string str) => str.Replace(@"\", "");
    }
}
