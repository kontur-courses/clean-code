using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class StringUtils
    {
        public static string Join(IEnumerable<string> tokens) => string.Join("", tokens);
        public static string Join(params Token[] tokens) => Join(tokens.Select(x => x.Value));
        public static string Join(IEnumerable<Token> tokens) => Join(tokens.Select(x => x.Value));
    }
}