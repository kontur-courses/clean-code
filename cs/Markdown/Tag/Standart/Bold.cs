namespace Markdown.Tag.Standart
{
    public class Bold : MarkdownTag
    {
        public Bold() : base("__", "bold", typeof(Italic)) { }
    }
}