using System;

namespace Markdown
{
    public class TagEm : Tag, IPairTag
    {
        public TagEm()
        {
            End = "_";
            Start = End;
            FindRule = (currentNode) => (currentNode.Value == Start && (!currentNode.Next?.Value.StartsWith(" ") ?? false));
            CloseRule = (currentNode) => (currentNode.Value == End && (!currentNode.Previous?.Value.EndsWith(" ") ?? false));
        }
        public override string ToString() => "em";
        public string StartTag { get; } = "<em>";
        public string EndTag { get; } = "</em>";
        public Func<Tag, bool> CanIContainThisTagRule { get; } = t => !(t is TagStrong);
    }
}
