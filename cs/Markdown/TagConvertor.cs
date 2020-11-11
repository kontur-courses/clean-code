using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TagConvertor
    {
        private static Dictionary<string, string> d = new Dictionary<string, string>()
        {
            {"_", "em"},
            {"__", "strong"},
            {"#", "h1"}
        };

        public static string ConvertMdToHtml(string tag)
        {
            if (d.TryGetValue(tag, out var convertedTag))
                return convertedTag;
            throw new Exception();
        }
    }
}