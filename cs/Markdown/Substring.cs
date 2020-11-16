using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Substring
    {
        public readonly string Value;
        public readonly int Index;
        public readonly int Length;
        public readonly int EndIndex;

        public Substring(int index, string value)
        {
            Value = value;
            Index = index;
            Length = value.Length;
            EndIndex = Index + Length - 1;
        }
    }
}
