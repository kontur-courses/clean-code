using System;

namespace Markdown
{
    public class TagStrong : Tag, IPairTag
    {
        public override string ToString() => "strong";
        public string StartTag { get; } = "<strong>";
        public string EndTag { get; } = "</strong>";
        public Func<Tag, bool> CanIContainThisTagRule { get; } = t => true;

    }
}
