using System;
using System.Linq;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawString)
        {
            if (rawString is null)
            {
                throw new ArgumentNullException($"{nameof(rawString)} can not be null");
            }
            var tokenizer = new Tokenizer();
            var markingParser = new MarkingTreeBuilder();
            var tokens = tokenizer.GetTokens(rawString).ToList();
            var nodes = markingParser.BuiltTree(tokens);
            return nodes.GetNodeBuilder().ToString();
        }
    }
}