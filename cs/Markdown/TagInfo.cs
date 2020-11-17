using System.Collections.Generic;

namespace Markdown
{
    public class TagInfo
    {
        public List<TagInfo> Content { get; }
        public string Text { get; private set; }
        public List<TagAttribute> Attributes { get; }
        public string Tail { get; private set; }
        public Tag Tag { get; private set; }
        public bool IsClosed { get; set; }
        public bool InsideWord { get; set; }

        public TagInfo(Tag tag = default, string text = null)
        {
            Tag = tag;
            Text = Escape(text);
            Content = new List<TagInfo>();
            Attributes = new List<TagAttribute>();
        }

        public void AddContent(TagInfo tagInfo)
        {
            if (tagInfo.Tag == Tag.NoFormatting && tagInfo.Text == "")
                return;
            Content.Add(tagInfo);
        }

        public void ResetFormatting(bool addTail = false)
        {
            if (Tag == Tag.Bold || Tag == Tag.Italic)
            {
                var underscores = Tag == Tag.Bold
                    ? UnderscoreParser.DoubleUnderscore
                    : UnderscoreParser.UnderscoreSymbol.ToString();
                Text = underscores + Text;
                if (addTail)
                    Tail = Tag == Tag.Bold
                        ? UnderscoreParser.DoubleUnderscore
                        : UnderscoreParser.UnderscoreSymbol.ToString();
            }
            else if (Tag == Tag.Link)
            {
                Text = '[' + Text;
                if (addTail)
                    Tail = "]";
            }

            Tag = default;
        }

        private static string Escape(string text)
        {
            if (text is null)
                return null;
            return text.Escape();
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

        public void AddAttribute(TagAttribute attribute)
        {
            Attributes.Add(attribute);
        }
    }
}