namespace Markdown
{
    public class Tag
    {
        public TagSide Side { get; }
        public TagKind Kind { get; }

        public Tag(TagSide side, TagKind kind)
        {
            Side = side;
            Kind = kind;
        }

    }
}