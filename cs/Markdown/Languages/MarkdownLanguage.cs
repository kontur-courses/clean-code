using Markdown.Tokenizing;

namespace Markdown.Languages
{
    public class MarkdownLanguage : Language
    {
        public MarkdownLanguage()
        {
            AddTag(Tag.Emphasize, "_", "_");
            AddTag(Tag.Strong, "__", "__");
        }
    }
}
