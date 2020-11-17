using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Markdown
{
    public static class StringExtensions
    {
        private static readonly IDictionary<string, string> Map = new Dictionary<string, string>
        {
            {$"\\{UnderscoreParser.UnderscoreSymbol}", $"{UnderscoreParser.UnderscoreSymbol}"},
            {$"\\{MarkdownParser.HashSymbol}", $"{MarkdownParser.HashSymbol}"},
            {$"\\{LinkParser.LinkOpenSymbol}", $"{LinkParser.LinkOpenSymbol}"},
            {$"\\{LinkParser.LinkCloseSymbol}", $"{LinkParser.LinkCloseSymbol}"},
            {$"\\{LinkParser.AttributeOpenSymbol}", $"{LinkParser.AttributeOpenSymbol}"},
            {$"\\{LinkParser.AttributeCloseSymbol}", $"{LinkParser.AttributeCloseSymbol}"},
            {@"\\", @"\"}
        };

        private static readonly Regex Replace = new Regex(string.Join("|", Map.Keys.Select(k => Regex.Escape(k))));

        public static string Escape(this string str)
        {
            return str is null ? null : Replace.Replace(str, m => Map[m.Value]);
        }
    }
}