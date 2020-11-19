namespace Markdown
{
    public class SingleTagToken : TagToken
    {
        public SingleTagToken(int startPosition, int endPosition, TagType type) : base(startPosition, endPosition, type)
        {
        }
    }
}