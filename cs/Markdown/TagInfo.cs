using System.Collections.Generic;

namespace Markdown
{
    public class TagInfo
    {
        public List<TagInfo> Content { get; }
        public string Text { get; private set; }
        public string Tail { get; private set; }
        public Tag Tag { get; private set; }
        public bool IsClosed { get; set; }
        public bool InsideWord { get; set; }


        public TagInfo(Tag tag = default, string text = null)
        {
            Tag = tag;
            Text = Escape(text);
            Content = new List<TagInfo>();
        }

        public void AddText(string text)
        {
            if (Content.Count != 0 || Text != null)
                Content.Add(new TagInfo(text: text));
            else
                Text = Escape(text);
        }

        public void AddContent(TagInfo tagInfo)
        {
            Content.Add(tagInfo);
        }

        public void ResetFormatting(bool addTail = false)
        {
            if (Tag != Tag.Bold && Tag != Tag.Italic)
                return;
            var underscores = Tag == Tag.Bold
                ? UnderscoreParser.DoubleUnderscore
                : UnderscoreParser.UnderscoreSymbol.ToString();
            Text = underscores + Text;
            if (addTail)
                Tail = Tag == Tag.Bold
                    ? UnderscoreParser.DoubleUnderscore
                    : UnderscoreParser.UnderscoreSymbol.ToString();
            Tag = default;
        }

        private static string Escape(string text)
        {
            return text?.Replace("\\\\", "\\")
                .Replace($"\\{UnderscoreParser.UnderscoreSymbol}", UnderscoreParser.UnderscoreSymbol.ToString())
                .Replace($"\\{MarkdownParser.HashSymbol}", MarkdownParser.HashSymbol.ToString());
        }

        public IEnumerable<TagInfo> FindAndGetBoldContent()
        {
            var list = new List<TagInfo>();
            foreach (var element in Content)
            {
                if (element.Tag == Tag.Bold && element.IsClosed)
                    list.Add(element);
                list.AddRange(element.FindAndGetBoldContent());
            }

            return list;
        }
    }
}