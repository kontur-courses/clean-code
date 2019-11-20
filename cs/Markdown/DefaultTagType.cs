namespace Markdown
{
    public class DefaultTagType : TagType
    {
        public override bool IsOpenedTag(string text, int position)
        {
            return Tag.GetTag(Tag.TagName.DefaultOpened, Tag.Markup.Md) == text[position].ToString();
        }

        public override bool IsClosedTag(string text, int position)
        {
            return Tag.GetTag(Tag.TagName.DefaultClosed, Tag.Markup.Md) == text[position].ToString();
        }

        public override string GetOpenedTag(Tag.Markup markup)
        {
            return Tag.GetTag(Tag.TagName.DefaultOpened, markup);
        }

        public override string GetClosedTag(Tag.Markup markup)
        {
            return Tag.GetTag(Tag.TagName.DefaultClosed, markup);
        }
    }
}