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
    }
}
