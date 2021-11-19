namespace Markdown
{
    public class Tag
    {
        public TagSide Side { get; }
        public TagKind Kind { get; }

        public Tag(TagKind kind, TagSide side)
        {
            Kind = kind;
            Side = side;
        }

    }
}