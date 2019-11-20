namespace Markdown
{
    public class StrongTagType : TagType
    {
        public StrongTagType() : base("<strong>", "</strong>", "__", "__")
        {
        }

        public override bool IsOpenedTag(string text, int position)
        {
            if (position + 2 >= text.Length || text[position] != '_' || text[position + 1] != '_')
                return false;
            if (position == 0)
                return char.IsLetter(text[position + 2]);
            return char.IsLetter(text[position + 2]) && text[position - 1] == ' ';
        }

        public override bool IsClosedTag(string text, int position)
        {
            if (text[position] != '_' || position < 1 || position + 1 >= text.Length || text[position + 1] != '_')
                return false;
            if (position + 1 == text.Length - 1)
                return char.IsLetter(text[position - 1]);
            return char.IsLetter(text[position - 1]) && text[position + 2] == ' ';
        }

        public override string GetOpenedTag(Tag.Markup markup)
        {
            return Tag.GetTag(Tag.TagName.StrongOpened, markup);
        }

        public override string GetClosedTag(Tag.Markup markup)
        {
            return Tag.GetTag(Tag.TagName.StrongClosed, markup);
        }
    }
}