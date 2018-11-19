using System.Text;
using Markdown.Data.Nodes;

namespace Markdown.TreeTranslator.NodeTranslator
{
    public interface INodeTranslator
    {
        void Translate(RootTreeNode node, StringBuilder textBuilder);
        void Translate(TextTreeNode node, StringBuilder textBuilder);
        void Translate(TagTreeNode node, StringBuilder textBuilder);
    }
}