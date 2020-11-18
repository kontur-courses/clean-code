namespace Markdown
{
    public class TagToHtmlReplacementInfo
    {
        public int Position;
        public TagType Type;
        public bool IsCloser;
        public int TagSignLength => TagAnalyzer.GetSignLength(Type);
        public string NewValue;

        public TagToHtmlReplacementInfo(int position, TagType type, bool isCloser)
        {
            Position = position;
            Type = type;
            IsCloser = isCloser;
            NewValue = $"<{(isCloser ? "/" : "")}{TagAnalyzer.GetHtmlValue(Type)}>";
        }
    }
}