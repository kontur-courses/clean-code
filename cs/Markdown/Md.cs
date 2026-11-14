using Markdown.MDParsing;

namespace Markdown
{
    public class Md
    {
        private readonly MarkdownParser _parser = new MarkdownParser();

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return string.Concat(_parser.Parse(input).Select(node => node.Render()));
        }
    }
}