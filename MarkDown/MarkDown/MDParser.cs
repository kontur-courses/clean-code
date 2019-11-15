using System;
using System.Collections.Generic;
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

        private static string CorrectIntersectingTags(string line) //TODO: Rewrite with custom class and remove nested tags
        {
            var tagStack = new Stack<TagParser>();
            var builder = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                var written = false;
                foreach (var parser in tagParsers) 
                {
                    if (i + parser.OpeningHtmlTag.Length < line.Length && 
                        line.Substring(i, parser.OpeningHtmlTag.Length).Equals(parser.OpeningHtmlTag)) //TokenParse?
                    {
                        i += parser.OpeningHtmlTag.Length - 1;
                        builder.Append(parser.OpeningHtmlTag);
                        tagStack.Push(parser);
                        written = true;
                        break;
                    }
                    if (i + parser.ClosingHtmlTag.Length < line.Length && 
                        line.Substring(i, parser.ClosingHtmlTag.Length).Equals(parser.ClosingHtmlTag))
                    {
                        i += parser.ClosingHtmlTag.Length - 1;
                        var reverseTraverseStack = new Stack<TagParser>();
                        do
                        {
                            reverseTraverseStack.Push(tagStack.Pop());
                            builder.Append(reverseTraverseStack.Peek().ClosingHtmlTag);
                            if (tagStack.Peek().Equals(parser))
                            {
                                builder.Append(tagStack.Peek().ClosingHtmlTag);
                                break;
                            }
                        } while (true);
                        do
                        {
                            tagStack.Push(reverseTraverseStack.Pop());
                            builder.Append(tagStack.Peek().OpeningHtmlTag);
                        } while (reverseTraverseStack.Count > 0);
                        written = true;
                        break;
                    }
                }
                if (!written)
                    builder.Append(line[i]);
            }
            return builder.ToString();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(ParseFrom(Console.ReadLine()));
        }
    }
}