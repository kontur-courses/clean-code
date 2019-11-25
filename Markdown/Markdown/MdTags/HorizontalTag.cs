using System;
using System.Collections.Generic;


namespace Markdown.MdTags
{
    internal class HorizontalTag: Tag
    {
        public override string OpenedMdTag { get; protected set; } = "***";
        protected override string OpenedHtmlTag { get;  set; } = "<hr>";

        public HorizontalTag((int lenght, string content) contentInfo = default) : base(contentInfo)
        { }

        public override bool CanClose(string tag) => false;

        public override bool CanOpen(Stack<Tag> stack, string content) 
            => (stack.Count == 0 )
               && (content == string.Empty || content.StartsWith(Environment.NewLine));

        public override void AutoClose(List<Tag> tags)
        {
            tags.Add(this);
        }
    }
}
