using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    public static class StringUtils
    {
        public static string Join(IEnumerable<string> tokens) => string.Join("", tokens);
        public static string Join(params ITextContainer[] tokens) => Join(tokens.Select(x => x.GetText()));
        public static string Join(IEnumerable<ITextContainer> tokens) => Join(tokens.Select(x => x.GetText()));
    }
}