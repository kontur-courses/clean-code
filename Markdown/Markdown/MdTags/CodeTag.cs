using System.Collections.Generic;

namespace Markdown.MdTags
{
    internal class CodeTag: Tag
    {
        public override string OpenedMdTag { get; protected set; } = "`";
        public override string ClosedMdTag { get; protected set; } = "`";
        protected override string OpenedHtmlTag { get; set; } = "<code>";
        protected override string ClosedHtmlTag { get; set; } = "</code>";

        public CodeTag((int lenght, string content) contentInfo) : base(contentInfo)
        { }

        public override bool CanOpen(Stack<Tag> stack, string content) => true;
    }
}
