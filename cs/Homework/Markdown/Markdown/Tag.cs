namespace Markdown
{
    public struct Tag
    {
        public readonly string MD;
        public readonly string HTML;

        public Tag(string md, string html)
        {
            MD = md;
            HTML = html;
        }
    }
}
