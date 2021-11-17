namespace Markdown
{
    public interface ITokenSegment
    {
        public abstract bool IsIntersectWith(ITokenSegment other);
    }
}