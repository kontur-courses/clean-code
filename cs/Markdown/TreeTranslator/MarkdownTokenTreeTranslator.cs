using System.Text;
using Markdown.Data.Nodes;
using Markdown.TreeTranslator.NodeTranslator;

namespace Markdown.TreeTranslator
{
    public class MarkdownTokenTreeTranslator : ITokenTreeTranslator
    {
        private readonly INodeTranslator nodeTranslator;

        public MarkdownTokenTreeTranslator(INodeTranslator nodeTranslator)
        {
            this.nodeTranslator = nodeTranslator;
        }

        public string Translate(TokenTreeNode tokenTree)
        {
            var textBuilder = new StringBuilder();
            tokenTree.Translate(nodeTranslator, textBuilder);
            return textBuilder.ToString();
        }
    }
}