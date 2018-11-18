using Markdown.Tokenizing;

namespace Markdown.Languages
{
    public class HtmlLanguage : Language
    {
        public HtmlLanguage()
        {
            AddTag(Tag.Emphasize, "<em>", "</em>");
            AddTag(Tag.Strong, "<strong>", "</strong>");
        }
    }
}
