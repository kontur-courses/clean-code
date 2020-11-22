using System.Text;

namespace Markdown
{
    public static class HtmlCreator
    {
        public static string AddHtmlTagToText(string text, Tag tag)
        {
            var textStringBuilder = new StringBuilder(text);
            if (tag.Markdown != null) textStringBuilder.Remove(tag.Position, tag.Markdown.Length);

            if (tag.Position > text.Length)
                textStringBuilder.Append(tag.Value);
            else
                textStringBuilder.Insert(tag.Position, tag.Value);

            return textStringBuilder.ToString();
        }
    }
}