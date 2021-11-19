namespace Markdown
{
    public interface ITokenSegment
    {
        Token GetBaseToken();
        bool IsIntersectWith(ITokenSegment other);
    }
}