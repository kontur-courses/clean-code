using System.Text;
using Markdown.TreeTranslator.NodeTranslator;

namespace Markdown.Data.Nodes
{
    public class TextTreeNode : TokenTreeNode
    {
        public readonly string Text;

        public TextTreeNode(string text)
        {
            Text = text;
        }

        public override void Translate(INodeTranslator translator, StringBuilder textBuilder) =>
            translator.Translate(this, textBuilder);
    }
}