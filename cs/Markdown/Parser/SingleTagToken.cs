namespace Markdown
{
    public class SingleTagToken : TagToken
    {
        public SingleTagToken(int startPosition, int endPosition, TagType type) : base(startPosition, endPosition, type)
        {
        }

        public override string GetHtmlValue(bool isCloser)
        {
            return Type is TagType.Shield ? "" : base.GetHtmlValue(isCloser);
        }
    }
}