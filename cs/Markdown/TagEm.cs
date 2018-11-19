using System;

namespace Markdown
{
    public class TagEm : Tag, IPairTag
    {
        public override string ToString() => "em";
        public string StartTag { get; } = "<em>";
        public string EndTag { get; } = "</em>";
        public Func<Tag, bool> CanIContainThisTagRule { get; } = t => !(t is TagStrong);
    }
}
