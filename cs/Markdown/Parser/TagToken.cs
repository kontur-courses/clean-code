namespace Markdown
{
    public class TagToken
    {
        public readonly int StartPosition;
        public readonly int ValueLength;
        public readonly TagType Type;
        public virtual int TagSignLength => TagAnalyzer.GetSignLength(Type);
        public readonly int EndPosition;

        public TagToken(int startPosition, int endPosition, TagType type)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Type = type;
            ValueLength = (type is TagType.Shield) ? 0 : endPosition - startPosition - TagSignLength;
        }

        public virtual int GetReplacedValueLength(bool isCloser) =>
            TagSignLength;

        public virtual string GetHtmlValue(bool isCloser)
        {
            var defaultHtmlValue = TagAnalyzer.GetDefaultHtmlValue(Type);
            return isCloser ? $"</{defaultHtmlValue}>" : $"<{defaultHtmlValue}>";
        }
    }
}