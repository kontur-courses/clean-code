using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Markdown
{
    public static class MarkdownParser
    {
        public static string Parse(string text)
        {
            var indexes = GetIndexTags(text);
            var textWithHtmlTags = ReplaceAllMDTags(indexes, text);
            return textWithHtmlTags;
        }

        private static List<(int openTagIndex, int closeTagIndex, int lenTag)> GetIndexTags(string text)
        {
            return new List<(int, int, int)>();
        }

        private static string ReplaceAllMDTags(List<(int, int, int)> indexes, string text)
        {
            var builder = new StringBuilder();
            foreach (var (startIndex, endIndex, lenTag) in indexes)
                builder.Append(ReplaceMDTag(startIndex, endIndex, lenTag, text));
            return builder.ToString();
        }

        private static string ReplaceMDTag(int startIndex, int endIndex, int lenTag, string text)
        {
            var builder = new StringBuilder();
            var mdTag = text.Substring(startIndex, lenTag);
            var stringWithoutTag = text.Substring(startIndex + lenTag, endIndex - (startIndex + lenTag));
            
            builder.Append(text.Substring(0, startIndex));
            builder.Append(HtmlTag.CreateStringWithHtmlTag(stringWithoutTag, Tags.TagsDictionary[mdTag]));
            builder.Append(text.Substring(endIndex));
            return builder.ToString();
        }
    }
}
