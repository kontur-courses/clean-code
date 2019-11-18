using System.Collections.Generic;

namespace Markdown.MdTags
{
    internal class CodeTag: Tag
    {
        public override string OpenedMdTag { get; protected set; } = "`";
        public override string ClosedMdTag { get; protected set; } = "`";
        public override string OpenedHtmlTag { get; protected set; } = "<code>";
        public override string ClosedHtmlTag { get; protected set; } = "</code>";

        public override bool CanOpen(Stack<Tag> stack, string content) 
            => !content.StartsWith(" ") && stack.Count == 0;

    }
}
