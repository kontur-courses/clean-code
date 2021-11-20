namespace Markdown
{
    public interface ITokenSegment
    {
        Tag GetBaseToken();
        bool IsIntersectWith(ITokenSegment other);
    }
}