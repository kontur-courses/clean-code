using System.Collections.Generic;

namespace Markdown.Shell
{
    public class MatchObject
    {
        public readonly int Start;
        public readonly int End;
        public readonly List<Attribute> Attributes;
        public int Length => End - Start + 1;
        public MatchObject(int start, int end, List<Attribute> attributes)
        {
            Start = start;
            End = end;
            Attributes = attributes;
        }
    }
}
