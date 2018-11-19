using System.Text;
using Markdown.TreeTranslator.NodeTranslator;

namespace Markdown.Data.Nodes
{
    public class RootTreeNode : TagTreeNode
    {
        public RootTreeNode() : base(null) { }
        public override void Translate(INodeTranslator translator, StringBuilder textBuilder) =>
            translator.Translate(this, textBuilder);
    }
}