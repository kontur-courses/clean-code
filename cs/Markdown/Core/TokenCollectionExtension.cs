using System.Collections.Generic;
using System.Linq;

namespace Markdown.Core
{
    public static class TokenCollectionExtension
    {
        public static string ConvertToHtmlString(this IEnumerable<IToken> tokens) =>
            string.Join("", tokens.Select(t => t.ToHtmlString()));
    }
}