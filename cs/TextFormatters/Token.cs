using System;
using System.Collections.Generic;
using System.Text;

namespace TextFormatters
{
    public class Token
    {
        public Line Line { get; private set; }
        public string Value { get; private set; }
        public int TotalStartIndex { get; private set; }
        public int TotalEndIndex { get; private set; }
        public int Length => TotalEndIndex - TotalStartIndex;

        public Token(Line line, string value, int totalStartIndex, int totalEndIndex)
        {
            Line = line;
            Value = value;
            TotalStartIndex = totalStartIndex;
            TotalEndIndex = totalEndIndex;
        }
    }
}
