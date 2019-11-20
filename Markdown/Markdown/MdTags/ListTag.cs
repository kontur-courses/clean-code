using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Markdown.MdTags
{
    internal class ListTag: Tag
    {
        public override string ClosedMdTag { get; protected set; } = "\n";

        private readonly List<string> allowable = new List<string>() { "*" };

        public override string OpenedMdTag { get; protected set; } = "*";
        public override string OpenedHtmlTag { get; protected set; } = "<li>";
        public override string ClosedHtmlTag { get; protected set; } = "</li>";

        public override (int lenght, string content) GetContent(int index, string text)
        {
            var length = 0;
            var content = new StringBuilder();
            for (var i = index; i < text.Length; i++)
            {
                if (OtherTagFound(text, i)) break;
                if (text[i] == '\\' && i != text.Length - 1)
                {
                    SlashHandler(ref i, ref length, content, text[i + 1]);
                    continue;
                }
                content.Append(text[i]);
            }

            return (length + content.Length, content.ToString());
        }

        private bool OtherTagFound(string text, int i)
            => (text[i].ToString() == ClosedMdTag) || (AllTags.Contains(text[i].ToString()));

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