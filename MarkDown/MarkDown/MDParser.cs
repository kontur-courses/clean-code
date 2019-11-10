using System;
using System.Collections.Generic;
using MarkDown.TagParsers;

namespace MarkDown
{
    public static class MDParser
    {
        private static readonly List<TagParser> tagParsers = new List<TagParser>()
        {
            new EmTagParser(),
            new StrongTagParser()
        };

        public static string ParseFrom(string line)
        {
            string result = null;
            foreach (var parser in tagParsers)
            {
                result = parser.GetParsedLineFrom(line);
            }
            return result;
        }
    }
}