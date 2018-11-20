using System;

namespace Markdown
{
    public class TagStrong : Tag, IPairTag
    {
        public TagStrong()
        {
            End = "__";
            Start = End;
            FindRule = (currentNode) => (currentNode.Value == Start && (!currentNode.Next?.Value.StartsWith(" ") ?? false));
            CloseRule = (currentNode) => (currentNode.Value == End && (!currentNode.Previous?.Value.EndsWith(" ") ?? false));
        }
        public override string ToString() => "strong";
        public string StartTag { get; } = "<strong>";
        public string EndTag { get; } = "</strong>";
        public Func<Tag, bool> CanIContainThisTagRule { get; } = t => true;

    }
}
