using System.Collections.Generic;


namespace Markdown.MdTags
{
    internal class HeaderTag: Tag
    {
        public override string ClosedMdTag { get; protected set; } = "\n";

        public HeaderTag(string mdTag = "")
        {
            OpenedMdTag = mdTag;
            OpenedHtmlTag = $"<h{mdTag.Length}>";
            ClosedHtmlTag = $"</h{mdTag.Length}>";
        }

        public override (int lenght, string content) GetContent(int index, string text)
        {
            var length = 0;
            var content = string.Empty;
            for (var i = index; i < text.Length; i++)
            {
                if (text[i].ToString() == ClosedMdTag) break;
                if (allTags.Contains(text[i].ToString())) break;
                if (text[i] == '\\' && i != text.Length - 1)
                {
                    SlashHandler(ref i, ref length, ref content, text[i + 1]);
                    continue;
                }
                content += text[i];
            }

            return (length + content.Length, content);
        }

        public override bool CanOpen(Stack<Tag> stack, string content)
            => !content.StartsWith(" ") && !int.TryParse(content, out _);

        public override void AutoClose(List<Tag> tags)
        {
            tags.Add(this);
        }
    }
}
