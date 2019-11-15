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

        private static string CorrectIntersectingTags(string line) //Probably needs to be simplified, but it will impact on performance
        {
            var tagParserStack = new TraverseStack<TagParser>();
            var builder = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                var written = false;
                foreach (var parser in tagParsers) 
                {
                    if (EqualsOpeningTag(line, i, parser))
                    {
                        if (IsAllowedToBeNested(parser, tagParserStack))
                        {
                            builder.Append(parser.OpeningHtmlTag);
                            tagParserStack.Push(parser);
                        }
                        else
                            builder.Append(parser.MdTag);
                        written = true;
                        i += parser.OpeningHtmlTag.Length - 1;
                        break;
                    }
                    if (EqualsClosingTag(line, i, parser))
                    {
                        if (tagParserStack.Contains(parser))
                        {
                            builder.Append(CollectNestedTagsAndCloseTag(parser, tagParserStack));
                            tagParserStack.Remove(parser);
                        }
                        else
                            builder.Append(parser.MdTag);
                        written = true;
                        i += parser.ClosingHtmlTag.Length - 1;
                        break;
                    }
                }
                if (!written)
                    builder.Append(line[i]);
            }
            return builder.ToString();
        }

        private static bool EqualsOpeningTag(string line, int index, TagParser parser)
        {
            return index + parser.OpeningHtmlTag.Length <= line.Length &&
                   line.Substring(index, parser.OpeningHtmlTag.Length).Equals(parser.OpeningHtmlTag);
        }

        private static bool EqualsClosingTag(string line, int index, TagParser parser)
        {
            return index + parser.ClosingHtmlTag.Length <= line.Length &&
                   line.Substring(index, parser.ClosingHtmlTag.Length).Equals(parser.ClosingHtmlTag);
        }

        private static bool IsAllowedToBeNested(TagParser parser, TraverseStack<TagParser> tagParserStack)
        {
            return !(parser.Equals(tagParsers[0]) && tagParserStack.Contains(tagParsers[1]));
        }

        private static string CollectNestedTagsAndCloseTag(TagParser currentParser,
            TraverseStack<TagParser> tagParserStack)
        {
            var builder = new StringBuilder();
            var traverse = tagParserStack.TraverseToElementAndReturnBack(currentParser).ToList();
            var traverseIndex = 0;
            for (; traverseIndex < (int)Math.Ceiling((double)traverse.Count / 2); traverseIndex++)
            {
                builder.Append(traverse[traverseIndex].ClosingHtmlTag);
            }
            for (; traverseIndex < traverse.Count; traverseIndex++)
                builder.Append(traverse[traverseIndex].OpeningHtmlTag);
            return builder.ToString();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(ParseFrom(Console.ReadLine()));
        }
    }
}