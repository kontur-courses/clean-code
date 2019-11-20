namespace Markdown
{
    public abstract class TagType
    {
        public abstract bool IsOpenedTag(string text, int position);
        public abstract bool IsClosedTag(string text, int position);
        public abstract string GetOpenedTag(Tag.Markup markup);
        public abstract string GetClosedTag(Tag.Markup markup);

        public static TagType GetTagType(string text, int position)
        {
            var em = new EmTagType();
            var strong = new StrongTagType();
            if (em.IsOpenedTag(text, position) || em.IsClosedTag(text, position))
                return em;
            if (strong.IsOpenedTag(text, position) || strong.IsClosedTag(text, position))
                return strong;
            return new DefaultTagType();
        }
    }
}