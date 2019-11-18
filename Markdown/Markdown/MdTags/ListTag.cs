using System.Collections.Generic;
using System.Linq;


namespace Markdown.MdTags
{
    internal class ListTag: Tag
    {
        private readonly List<string> allowable = new List<string>() { "*" };

        public override string OpenedMdTag { get; protected set; } = "*";
        public override string OpenedHtmlTag { get; protected set; } = "<li>";
        public override string ClosedHtmlTag { get; protected set; } = "</li>";

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