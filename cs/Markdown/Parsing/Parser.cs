using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public class Parser
    {
        public static Predicate<string> DefaultTrueValidator => _ => true;

        public static IEnumerable<string> FindIter(
            string inputString,
            string patternString,
            Predicate<string> patternValidator = null,
            int start = 0,
            int length = 0)
        {
            throw new NotImplementedException();
        }

        public static Token FindToken(
            string inputString,
            string patternStrint,
            int start = 0,
            int length = 0)
        {
            throw new NotImplementedException();
        }
    }
}