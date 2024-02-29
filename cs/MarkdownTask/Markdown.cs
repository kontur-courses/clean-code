using MarkdownTask.HtmlTools;

namespace MarkdownTask
{
    internal class Markdown
    {
        private readonly IMarkdownParser[] Parsers;

        public Markdown(IMarkdownParser[] parsers)
        {
            Parsers = parsers;
        }

        public string Render(string markdownText)
        {
            var tokens = new List<Token>();

            foreach (var parser in Parsers)
            {
                tokens.AddRange(parser.Parse(markdownText));
            };

            return HtmlProcessor.Process(markdownText, tokens); ;
        }
    }
}