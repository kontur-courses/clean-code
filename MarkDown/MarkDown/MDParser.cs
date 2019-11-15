using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MarkDown.TagParsers;

namespace MarkDown
{
    public static class MdParser
    {
        private static readonly List<TagParser> tagParsers = new List<TagParser>()
        {
            new StrongTagParser(),
            new EmTagParser()
        };

        public static string ParseFrom(string line)
        {
            var result = EscapeHtmlTags(line);
            foreach (var parser in tagParsers)
            {
                result = parser.GetParsedLineFrom(result);
            }
            result = CorrectIntersectingTags(result);
            return RemoveEscapeChars(result);
        }

        private static string EscapeHtmlTags(string line)
        {
            return WebUtility.HtmlEncode(line);
        }

        private static string RemoveEscapeChars(string line)
        {
            var result = line;
            foreach (var parser in tagParsers)
            {
                result = result.Replace("\\" + parser.MdTag, parser.MdTag);
            }
            result = result.Replace("\\\\", "\\");
            return result;
        }

        private static string CorrectIntersectingTags(string line)
        {
            var tagParserStack = new TraverseStack<TagParser>();
            var builder = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                var written = false;
                foreach (var parser in tagParsers) 
                {
                    if (i + parser.OpeningHtmlTag.Length < line.Length && 
                        line.Substring(i, parser.OpeningHtmlTag.Length).Equals(parser.OpeningHtmlTag))
                    {
                        i += parser.OpeningHtmlTag.Length - 1;
                        if (!(parser.Equals(tagParsers[0]) && tagParserStack.Contains(tagParsers[1])))
                        {
                            builder.Append(parser.OpeningHtmlTag);
                            tagParserStack.Push(parser);
                            written = true;
                            break;
                        }
                    }
                    if (i + parser.ClosingHtmlTag.Length < line.Length && 
                        line.Substring(i, parser.ClosingHtmlTag.Length).Equals(parser.ClosingHtmlTag))
                    {
                        i += parser.ClosingHtmlTag.Length - 1;
                        if (tagParserStack.Contains(parser))
                        {
                            var traverse = tagParserStack.TraverseToElementAndReturnBack(parser).ToList();
                            var traverseIndex = 0;
                            for (; traverseIndex < (int)Math.Ceiling((decimal)(traverse.Count / 2)); traverseIndex++)
                            {
                                builder.Append(traverse[traverseIndex].ClosingHtmlTag);
                            }
                            for (; traverseIndex < traverse.Count; traverseIndex++)
                                builder.Append(traverse[traverseIndex].OpeningHtmlTag);
                            break;
                        }
                    }
                }
                if (!written)
                    builder.Append(line[i]);
            }
            return builder.ToString();
        } //Decompose, refactor and debug

        public static void Main(string[] args)
        {
            Console.WriteLine(ParseFrom(Console.ReadLine()));
        }
    }
}