using System;

namespace Markdown
{
    public class TagA : Tag, IPairTag
    {
        private string herf;
        public TagA()
        {
            End = ")";
            Start = "[";
            FindRule = (currentNode) => currentNode.Value == Start;
            CloseRule = (currentNode) =>
            {
                if (currentNode?.Value != End) return false;

                herf = currentNode?.Previous?.Value;



                if (currentNode?.Previous?.Previous?.Previous != null)
                    currentNode.Previous.Value = currentNode.Previous.Previous.Value =
                        currentNode.Previous.Previous.Previous.Value = "";
                return true;
            };
        }
        public override string ToString() => "a";
        public string StartTag => $"<a href=\"{herf}\">";
        public string EndTag { get; } = "</a>";
        public Func<Tag, bool> CanIContainThisTagRule { get; } = t => true;
    }
}
