namespace Markdown
{
    public record BlockTag(string Prefix, string TagName)
    {
        public static readonly BlockTag Header = new ("# ", "h1");
        public static readonly BlockTag ListItem = new ("- ", "li");
    }
}
