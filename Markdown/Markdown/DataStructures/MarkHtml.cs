namespace Markdown
{
    public class MarkHtml : MarkTranslatorElement
    {
        public MarkHtml(string openingMark, string closingMark, string tag, bool canBeParent = true)
            : base(new MarkupElement(openingMark, closingMark),
                new MarkupElement(tag, tag.Insert(1, "/")), canBeParent)
        {
        }

        public MarkHtml(string mark, string tag, bool canBeParent = true) : this(mark, mark, tag, canBeParent)
        {
        }
    }
}