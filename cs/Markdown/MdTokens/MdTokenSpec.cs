using System.Collections.Generic;

namespace Markdown.MdTokens
{
    public class MdTokenSpec
    {
        public string Tag { get; }
        public bool IsPaired { get; }
        public bool CanBeNested { get; }
        public HashSet<string> NestingExceptions { get; }

        public MdTokenSpec(string tag, bool isPaired, bool canBeNested)
        {
            Tag = tag;
            IsPaired = isPaired;
            CanBeNested = canBeNested;
            NestingExceptions = new HashSet<string>();
        }

        public void AddNestingExceptions(string tag)
        {
            NestingExceptions.Add(tag);
        }
    }
}