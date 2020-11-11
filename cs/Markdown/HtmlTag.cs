namespace Markdown
{
    public class HtmlTag
    {
        public readonly string Opening;
        public readonly string Ending;

        public HtmlTag(string name)
        {
            Opening = $"<{name}>";
            Ending = $"</{name}>";
        }
    }
}
