namespace Markdown
{
    public class Markup
    {
        public Markup(string name, string template, string HTMLTag)
        {
            Name = name;
            Template = template;
            this.HTMLTag = HTMLTag;
        }

        public string Name { get; }
        public string Template { get; }
        public string HTMLTag { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}