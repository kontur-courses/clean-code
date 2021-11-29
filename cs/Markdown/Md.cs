using System;
using System.Linq;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly MarkingTreeBuilder treeBuilder = new MarkingTreeBuilder();
        
        public string Render(string rawString)
        {
            if (rawString is null)
            {
                throw new ArgumentNullException($"{nameof(rawString)} can not be null");
            }
            var tokens = tokenizer.GetTokens(rawString).ToList();
            var topTreeNode = treeBuilder.BuiltTree(tokens);
            return topTreeNode.GetNodeBuilder().ToString();
        }
    }
}