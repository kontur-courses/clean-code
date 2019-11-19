using System.Text;
using Markdown.SeparatorConverters;
using Markdown.SyntaxAnalysis.SyntaxTree;

namespace Markdown.SyntaxAnalysis.SyntaxTreeConverters
{
    public class SyntaxTreeConverter : ISyntaxTreeConverter
    {
        public string Convert(SyntaxTree.SyntaxTree syntaxTree, ISeparatorConverter separatorConverter)
        {
            return RenderTreeNode(syntaxTree.Root, separatorConverter);
        }

        private string RenderTreeNode(SyntaxTreeNode treeNode, ISeparatorConverter separatorConverter)
        {
            var builder = new StringBuilder();
            if (treeNode.Token.IsSeparator && treeNode.IsClosed)
            {
                var newBuilder = new StringBuilder();
                foreach (var treeNodeChild in treeNode.GetChildren())
                {
                    newBuilder.Append(RenderTreeNode(treeNodeChild, separatorConverter));
                }

                builder.Append(separatorConverter.ConvertSeparator(treeNode.Token.Value, newBuilder.ToString()));
            }
            else
            {
                builder.Append(treeNode.Token.Value);
                foreach (var treeNodeChild in treeNode.GetChildren())
                {
                    builder.Append(RenderTreeNode(treeNodeChild, separatorConverter));
                }
            }

            return builder.ToString();
        }
    }
}