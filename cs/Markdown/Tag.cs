namespace Markdown
{
    public class Tag
    {
        public MarkSide Side { get; }
        public MarkKind Kind { get; }

        public Tag(MarkSide side, MarkKind kind)
        {
            Side = side;
            Kind = kind;
        }

    }
}