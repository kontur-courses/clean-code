using System.Text;

namespace Markdown.TagConverters
{
    internal class TagInfo
    {
        internal readonly TagConverterBase tagConverter;
        internal bool CanOpen => tagConverter.CanOpenBase(text, pos);
        internal bool CanClose => tagConverter.CanCloseBase(text, pos);
        private readonly StringBuilder text;
        internal int pos { get; }
        internal StringBuilder Convert(StringBuilder tagText, TagInfo pairTag) =>
            tagConverter.Convert(tagText, text, pos, pairTag.pos);

        internal TagInfo(TagConverterBase tagConverter, StringBuilder text, int pos)
        {
            this.tagConverter = tagConverter;
            this.text = text;
            this.pos = pos;
        }
    }
}
