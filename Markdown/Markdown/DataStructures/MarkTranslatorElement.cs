namespace Markdown
{
    public class MarkTranslatorElement
    {
        public static MarkTranslatorElement EmptyMark = new MarkTranslatorElement(
            new MarkupElement("", ""),
            new MarkupElement("",""));
        public readonly MarkupElement From;
        public readonly MarkupElement To;
        public readonly bool CanBeParent;
        
        public MarkTranslatorElement(MarkupElement from, MarkupElement to, bool canBeParent = true)
        {
            From = from;
            To = to;
            CanBeParent = canBeParent;
        }
    }
}