using System.Text;

namespace Markdown
{
    public static class HtmlMaker
    {
        public static string FromTextInfo(TagInfo tagInfo)
        {
            var result = new StringBuilder();
            var tagMaker = new TagMaker(tagInfo);
            result.Append(tagMaker.GetTextForOpeningTag());
            result.Append(tagMaker.GetTextForContent());
            result.Append(tagMaker.GetTextForClosingTag());
            return result.ToString();
        }
    }
}