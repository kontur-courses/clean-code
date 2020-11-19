namespace Markdown
{
    public class TagToHtmlReplacement
    {
        public readonly int Position;
        public readonly TagType Type;
        public readonly bool IsCloser;
        public readonly int ReplacedValueLength;
        public int TagSignLength => TagAnalyzer.GetSignLength(Type);
        public readonly string NewValue;

        public TagToHtmlReplacement(TagToken token, bool isCloser)
        {
            Position = isCloser ? token.EndPosition : token.StartPosition;
            Type = token.Type;
            IsCloser = isCloser;
            NewValue = token.GetHtmlValue(isCloser);
            ReplacedValueLength = token.TagSignLength;
        }
    }
}