namespace Markdown
{
    public class TagKeeper
    {
        public Tag Html { get; }
        public Tag Md { get; }

        public TagKeeper(Tag html, Tag md)
        {
            Html = html;
            Md = md;
        }

        public bool Is(string tag)
            => Html.Value.Equals(tag) || Md.Value.Equals(tag);

        public bool Is(TagKeeper tagKeeper)
            => Equals(tagKeeper);

        public override bool Equals(object obj)
        {
            var otherTagKeeper = (TagKeeper) obj;
            if (otherTagKeeper == null)
                return false;
            return Html.Equals(otherTagKeeper.Html)
                   && Md.Equals(otherTagKeeper.Md);
        }

        public bool Contains(string value)
            => Html.Value.Contains(value) || Md.Value.Contains(value);
    }
}
