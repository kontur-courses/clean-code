using Markdown.Tag.Standart;

namespace Markdown.Tag.Asterisks
{
    public class AsterisksBold : MarkdownTag
    {
        public AsterisksBold() : base("**", "bold", new Italic().Translation) { }
    }
}