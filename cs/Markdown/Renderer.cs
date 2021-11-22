using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Renderer
    {
        public static string Render(this IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
                result.Append(token.GetValue());
            return result.ToString();
        }
    }
}
