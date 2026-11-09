using Markdown.Abstractions;
using Markdown.Parsing;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{

    public class Md
    {
        private readonly MarkdownParser _parser;

        public Md()
        {
            _parser = new MarkdownParser();
        }

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            List<MarkdownNode> nodes = _parser.Parse(input);

            return string.Join("", nodes.Select(node => node.Render()));
        }
    }
}