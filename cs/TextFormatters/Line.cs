using System;
using System.Collections.Generic;
using System.Text;

namespace TextFormatters
{
    public class Line
    {
        public string Value { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public int Length => EndIndex - StartIndex;

        public Line(string value, int startIndex, int endIndex)
        {
            Value = value;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
