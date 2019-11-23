using System.Collections.Generic;

namespace Markdown.MdTags
{
    internal class EmTag : Tag
    {
        private readonly List<string> allowable = new List<string>() { "__", "_", "~", ">" };
        public override string OpenedMdTag { get; protected set; } = "_";
        public override string ClosedMdTag { get; protected set; } = "_";
        protected override string OpenedHtmlTag { get; set; } = "<em>";
        protected override string ClosedHtmlTag { get; set; } = "</em>";

        public EmTag((int lenght, string content) contentInfo) : base(contentInfo)
        { }

        public override bool CanOpen(Stack<Tag> stack, string content)
            => !content.StartsWith(" ") && (stack.Count == 0 || allowable.Contains(stack.Peek().OpenedMdTag))
                                        && !int.TryParse(content, out _);
    }
}
