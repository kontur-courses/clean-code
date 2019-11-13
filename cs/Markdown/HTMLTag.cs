namespace Markdown
{
    public class HTMLTag
    {
        public string Name { get; }
        public string First => $"<{Name}>";
        public string Second => $"</{Name}>";

        public HTMLTag(string name)
        {
            Name = name;
        }
    }
}