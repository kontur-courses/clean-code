using System.Collections.Generic;

namespace Markdown.MdTags
{
    internal class StrikeTag: Tag
    {
        private readonly List<string> allowable = new List<string>() { "__", "~" };
        public override string OpenedMdTag { get; protected set; } = "~";
        public override string ClosedMdTag { get; protected set; } = "~";
        public override string OpenedHtmlTag { get; protected set; } = "<strike>";
        public override string ClosedHtmlTag { get; protected set; } = "</strike>";

        public override bool CanOpen(Stack<Tag> stack, string content)
            => !content.StartsWith(" ") && (stack.Count == 0 || allowable.Contains(stack.Peek().OpenedMdTag))
                                        && !int.TryParse(content, out _);
    }
}
