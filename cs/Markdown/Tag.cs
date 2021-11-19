namespace Markdown
{
    public class Tag
    {
        public TagSide Side { get; }
        public MarkKind Kind { get; }

        public Tag(TagSide side, MarkKind kind)
        {
            Side = side;
            Kind = kind;
        }

    }
}