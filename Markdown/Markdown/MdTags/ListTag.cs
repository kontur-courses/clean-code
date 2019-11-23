using System.Collections.Generic;
using System.Linq;


namespace Markdown.MdTags
{
    internal class ListTag: Tag
    {
        private readonly List<string> allowable = new List<string>() { "*", "_", "__", "~" };
        public override string ClosedMdTag { get; protected set; } = "\n";
        public override string OpenedMdTag { get; protected set; } = "*";
        protected override string OpenedHtmlTag { get; set; } = "<li>";
        protected override string ClosedHtmlTag { get; set; } = "</li>";

        public ListTag((int lenght, string content) contentInfo) : base(contentInfo)
        { }

        public override bool CanClose(string tag) => false;

        public override bool CanOpen(Stack<Tag> stack, string content)
            => (stack.Count == 0 || allowable.Contains(stack.Peek().OpenedMdTag)) 
               && !content.StartsWith(" ") && !int.TryParse(content, out _);

        public override void AutoClose(List<Tag> tags)
        {
            if (tags.Count == 0)
            {
                tags.Add(this);
                return;
            }
            var nested = tags;
            while (nested.Last().NestedTags.Count != 0)
                nested = nested.Last().NestedTags;
            nested.Last().NestedTags.Add(this);
        }
    }
}