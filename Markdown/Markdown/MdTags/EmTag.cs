using System.Collections.Generic;

namespace Markdown.MdTags
{
    internal class EmTag : Tag
    {
        private readonly List<string> allowable = new List<string>() { "__", "_", "~", ">" };
        public override string OpenedMdTag { get; protected set; } = "_";
        public override string ClosedMdTag { get; protected set; } = "_";
        public override string OpenedHtmlTag { get; protected set; } = "<em>";
        public override string ClosedHtmlTag { get; protected set; } = "</em>";

        public override bool CanOpen(Stack<Tag> stack, string content)
            => !content.StartsWith(" ") && (stack.Count == 0 || allowable.Contains(stack.Peek().OpenedMdTag))
                                        && !int.TryParse(content, out _);
    }
}
