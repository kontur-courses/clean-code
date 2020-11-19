namespace Markdown
{
    public class TagToHtmlReplacement
    {
        public readonly int Position;
        public readonly int ReplacedValueLength;
        public readonly string NewValue;

        public TagToHtmlReplacement(TagToken token, bool isCloser)
        {
            Position = isCloser ? token.EndPosition : token.StartPosition;
            NewValue = token.GetHtmlValue(isCloser);
            ReplacedValueLength = token.GetReplacedValueLength(isCloser);
        }
    }
}