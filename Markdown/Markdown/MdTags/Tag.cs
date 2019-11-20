using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.MdTags
{
    public abstract class Tag
    {
        protected static readonly List<string> AllTags = new List<string>()
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
            "***",
            "___",
            ">"
        };
        public virtual string ClosedMdTag { get;  protected set; }
        public virtual string OpenedMdTag { get; protected set; }
        public virtual string ClosedHtmlTag { get; protected set; }
        public virtual string OpenedHtmlTag { get; protected set; }
        public virtual string Content { get; protected set; }

        public readonly List<Tag> NestedTags = new List<Tag>();

        public virtual string WrapTagIntoHtml()
        {
            var finalString = new StringBuilder();
            finalString.Append(OpenedHtmlTag + Content);
            NestedTags.ForEach(tag => finalString.Append(tag.WrapTagIntoHtml()));
            finalString.Append(ClosedHtmlTag);
            return finalString.ToString();
        }

        public virtual void AutoClose(List<Tag> tags)
        {
            if (tags.Count == 0) tags.Add(new SimpleTag(OpenedMdTag));
            else tags.Last().NestedTags.Add(new SimpleTag(OpenedMdTag));
            tags.Last().NestedTags.AddRange(NestedTags);
        }

        public virtual bool CanOpen(Stack<Tag> stack, string content) => false;

        public virtual bool CanClose(string tag)
            => ClosedMdTag == tag && !NestedTags.Last().Content.EndsWith(" ");

        protected virtual void SlashHandler(ref int i, ref int length, StringBuilder content, char symbolToAdd)
        {
            content.Append(symbolToAdd);
            i++;
            length++;
        }

        public virtual (int lenght, string content) GetContent(int index, string text)
        {
            var length = 0;
            var content = new StringBuilder();
            for (var i = index; i < text.Length; i++)
            {
                if (AllTags.Contains(text[i].ToString())) break;
                if (text[i] == '\\' && i != text.Length - 1)
                {
                    SlashHandler(ref i, ref length, content, text[i + 1]);
                    continue;
                }
                content.Append(text[i]);
            }

            return (length + content.Length, content.ToString());
        }
    }
}
