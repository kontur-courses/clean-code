using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.MdTags
{
    public abstract class Tag
    {
        public static readonly List<string> AllTags = new List<string>()
        {
            "_",
            "__",
            "`",
            "~",
            "#",
            "##",
            "###",
            "####",
            "#####",
            "######",
            "*",
            "_",
            "**",
            "***",
            "___",
            ">"
        };
        public virtual string ClosedMdTag { get; protected set; } = string.Empty;
        public virtual string OpenedMdTag { get; protected set; } = string.Empty;
        protected virtual string ClosedHtmlTag { get; set; }
        protected virtual string OpenedHtmlTag { get; set; }
        public virtual string Content { get; protected set; }

        public int ContentLength { get; protected set; }

        public readonly List<Tag> NestedTags = new List<Tag>();

        protected Tag((int lenght, string content) contentInfo)
        {
            var (lenght, content) = contentInfo;
            Content = content;
            ContentLength = lenght;
        }

        public string WrapTagIntoHtml()
        {
            var finalString = new StringBuilder();
            finalString.Append(OpenedHtmlTag + Content);
            NestedTags.ForEach(tag => finalString.Append(tag.WrapTagIntoHtml()));
            finalString.Append(ClosedHtmlTag);
            return finalString.ToString();
        }

        public virtual void AutoClose(List<Tag> tags)
        {
            var newTag = new SimpleTag((OpenedMdTag.Length + ContentLength, OpenedMdTag + Content));
            if (tags.Count == 0)
                tags.Add(newTag);
            else
                tags.Last().NestedTags.Add(newTag);
            tags.Last().NestedTags.AddRange(NestedTags);
        }

        public virtual bool CanOpen(Stack<Tag> stack, string content) => false;

        public virtual bool CanClose(string tag)
            => ClosedMdTag == tag && ((NestedTags.Count != 0 && !NestedTags.Last().Content.EndsWith(" ") 
                                       || NestedTags.Count == 0 && !Content.EndsWith(" ")));
    }
}
