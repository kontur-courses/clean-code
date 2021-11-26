using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    public static class StringUtils
    {
        public static string Join(IEnumerable<string> strings) => string.Join("", strings);

        public static string Join(params ITextContainer[] textContainers) => Join(textContainers.AsEnumerable());
        
        public static string Join(IEnumerable<ITextContainer> textContainers) =>
            Join(textContainers.Select(x => x.GetText()));
    }
}