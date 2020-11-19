namespace Markdown
{
    public class TagToHtmlReplacement
    {
        public readonly int Position;
        public readonly TagType Type;
        public readonly bool IsCloser;
        public int TagSignLength => TagAnalyzer.GetSignLength(Type);
        public readonly string NewValue;

        public TagToHtmlReplacement(int position, TagType type, bool isCloser)
        {
            Position = position;
            Type = type;
            IsCloser = isCloser;
            if (type is TagType.Shield)
                NewValue = "";
            else
                NewValue = $"<{(isCloser ? "/" : "")}{TagAnalyzer.GetHtmlValue(Type)}>";
        }
    }
}