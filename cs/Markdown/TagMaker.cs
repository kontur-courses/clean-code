using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagMaker
    {
        private static readonly Dictionary<Tag, string> OpeningTag = new Dictionary<Tag, string>
        {
            {Tag.NoFormatting, ""},
            {Tag.Bold, "<strong>"},
            {Tag.Italic, "<em>"},
            {Tag.Heading, "<h1>"},
            {Tag.Link, "<a>"}
        };

        private static readonly Dictionary<Tag, string> ClosingTag = new Dictionary<Tag, string>
        {
            {Tag.NoFormatting, ""},
            {Tag.Bold, "</strong>"},
            {Tag.Italic, "</em>"},
            {Tag.Heading, "</h1>"},
            {Tag.Link, "</a>"}
        };

        private readonly TagInfo tagInfo;

        public TagMaker(TagInfo tagInfo)
        {
            this.tagInfo = tagInfo;
        }

        public string GetTextForOpeningTag()
        {
            if (tagInfo.Attributes.Count != 0)
            {
                var opening = new StringBuilder();
                var tag = OpeningTag[tagInfo.Tag];
                opening.Append(tag.Substring(0, tag.Length - 1));
                foreach (var attribute in tagInfo.Attributes)
                    opening.Append($" {attribute.Type.ToString().ToLower()}=\"{attribute.Text}\"");
                opening.Append(">");
                return opening.ToString();
            }

            return OpeningTag[tagInfo.Tag];
        }

        public string GetTextForContent()
        {
            var content = new StringBuilder();
            content.Append(tagInfo.Text);
            foreach (var element in tagInfo.Content)
                content.Append(HtmlMaker.FromTextInfo(element));
            content.Append(tagInfo.Tail);
            return content.ToString();
        }

        public string GetTextForClosingTag()
        {
            return ClosingTag[tagInfo.Tag];
        }
    }
}