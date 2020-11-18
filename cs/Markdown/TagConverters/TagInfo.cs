using System.Text;

namespace Markdown.TagConverters
{
    internal class TagInfo
    {
        internal readonly ITagConverter tagConverter;
        internal bool CanOpen => tagConverter.CanOpen(text, Pos);
        internal bool CanClose => tagConverter.CanClose(text, Pos);
        private readonly StringBuilder text;
        internal int Pos { get; }
        internal StringBuilder Convert(StringBuilder tagText, TagInfo pairTag) =>
            tagConverter.Convert(tagText, text, Pos, pairTag.Pos);

        internal TagInfo(ITagConverter tagConverter, StringBuilder text, int pos)
        {
            this.tagConverter = tagConverter;
            this.text = text;
            Pos = pos;
        }
    }
}
