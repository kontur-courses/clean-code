namespace Markdown
{
    public class HtmlTag : ITag
    {
        public string Name { get; }
        public static HtmlTag Empty { get; } = new HtmlTag("");
        public bool IsEmpty => Name.Length == 0;

        public string GetOpener() => IsEmpty
            ? ""
            : $"<{Name}>";

        public string GetClosing() => IsEmpty
            ? ""
            : $"</{Name}>";

        public HtmlTag(string name)
        {
            Name = name;
        }

        public string EncloseInTags(string line)
            => $"{GetOpener()}{line}{GetClosing()}";

        public string EncloseInTags(Token token)
            => EncloseInTags(token.Value);
    }
}