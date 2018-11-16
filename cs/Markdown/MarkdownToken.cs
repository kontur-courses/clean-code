namespace Markdown
{
    public class MarkdownToken
    {
        public MarkdownToken(string name, string template, string htmlTag)
        {
            Name = name;
            Template = template;
            HtmlTag = htmlTag;
        }

        public string Name { get; }
        public string Template { get; }
        public string HtmlTag { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}