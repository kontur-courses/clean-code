using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using MarkDown.TagParsers;

namespace MarkDown
{
    public static class Md
    {
        private static readonly List<Tag> tags = new List<Tag>()
        {
            new EmTag(),
            new StrongTag()
        };

        public static string Render(string line)
        {
            var escapedLine = EscapeHtmlTags(line);
            var tokens = TagParser.Tokenize(escapedLine);
            tokens = tags.OrderByDescending(x => x.MdTag.Length).Aggregate(tokens, TagParser.ParseTokensWithTag);
            tokens = TagParser.FindPairTags(tokens);
            tokens = TagParser.CorrectIntersectingTags(tokens);
            var result = TagParser.TokensToString(tokens);
            return RemoveEscapeChars(result);
        }

        private static string EscapeHtmlTags(string line)
        {
            return WebUtility.HtmlEncode(line);
        }

        private static string RemoveEscapeChars(string line)
        {
            var result = line;
            foreach (var parser in tags)
            {
                result = result.Replace("\\" + parser.MdTag, parser.MdTag);
            }
            result = result.Replace("\\\\", "\\");
            return result;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(Render(Console.ReadLine()));
        }
    }
}