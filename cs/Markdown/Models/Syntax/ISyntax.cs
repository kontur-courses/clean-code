using Markdown.Models.Tags;

namespace Markdown.Models.Syntax
{
    internal interface ISyntax
    {
        public bool TryGetTag(string text, int position, out TagInfo tagInfo);
        public bool IsStartParagraphTag(Tag tag);
        public bool IsEscapingTag(Tag tag);
        public bool IsValidAsOpening(TagInfo tag, string text);
        public bool IsValidAsClosing(TagInfo tag, string text);
        public bool IsValidAsInner(Tag source, Tag possibleInner);
        public bool IsInWord(PairedTag tag, string text);
    }
}
