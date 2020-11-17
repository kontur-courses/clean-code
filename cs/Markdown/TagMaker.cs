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
            {Tag.Heading, "<h1>"}
        };

        private static readonly Dictionary<Tag, string> ClosingTag = new Dictionary<Tag, string>
        {
            {Tag.NoFormatting, ""},
            {Tag.Bold, "</strong>"},
            {Tag.Italic, "</em>"},
            {Tag.Heading, "</h1>"}
        };

        private readonly TagInfo tagInfo;

        public TagMaker(TagInfo tagInfo)
        {
            this.tagInfo = tagInfo;
        }

        public string GetTextForOpeningTag()
        {
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