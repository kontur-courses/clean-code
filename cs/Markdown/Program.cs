namespace Markdown
{
    internal class Program
    {
        static void Main()
        {
            var tokenizer = new DefaultTokenizer<MarkdownTag>();
            var renderer = new DefaultRenderer<HTMLTag>();
            var rules = new DefaultRules();

            var md = new Md(tokenizer, renderer, rules);
            var markdownText = string.Empty;
            md.Render(markdownText);
        }
    }
}
