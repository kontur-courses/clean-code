using System.Collections.Generic;


namespace Markdown.MdTags
{
    internal class HeaderTag: Tag
    {
        public override string ClosedMdTag { get; protected set; } = "\n";

        public HeaderTag((int lenght, string content) contentInfo, string mdTag = "") : base(contentInfo)
        {
            OpenedMdTag = mdTag;
            OpenedHtmlTag = $"<h{mdTag.Length}>";
            ClosedHtmlTag = $"</h{mdTag.Length}>";
        }

        public override bool CanOpen(Stack<Tag> stack, string content)
            =>(stack.Count == 0 || stack.Count != 0 && stack.Peek().ClosedMdTag == ClosedMdTag) 
              && !content.StartsWith(" ") && !int.TryParse(content, out _);

        public override void AutoClose(List<Tag> tags)
        {
            tags.Add(this);
        }
    }
}
