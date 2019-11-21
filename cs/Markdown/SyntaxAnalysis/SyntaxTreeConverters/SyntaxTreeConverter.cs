using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.SeparatorConverters;
using Markdown.SyntaxAnalysis.SyntaxTreeRealization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeConverters
{
    public class SyntaxTreeConverter : ISyntaxTreeConverter
    {
        public string Convert(SyntaxTree syntaxTree, ISeparatorConverter separatorConverter)
        {
            return RenderSyntaxTree(syntaxTree, separatorConverter);
        }

        private string RenderSyntaxTree(SyntaxTree tree, ISeparatorConverter separatorConverter)
        {
            var builder = new StringBuilder();
            var formats = new List<List<string>>();
            var currentIndexesInFormats = new List<int>();

            foreach (var syntaxTreeNode in tree.Enumerate())
            {
                if (IsCorrectSeparatorInNode(syntaxTreeNode))
                {
                    AddFormatToFormats(formats, currentIndexesInFormats, syntaxTreeNode, separatorConverter);
                }
                else
                {
                    if (syntaxTreeNode.Token.IsSeparator && formats.Count > 0)
                    {
                        DeleteFormatFromFormats(formats, currentIndexesInFormats);
                    }
                    else if (formats.Count == 0)
                    {
                        builder.Append(syntaxTreeNode.Token.Value);
                    }
                    else
                    {
                        ApplyFormatsToNode(formats, currentIndexesInFormats, syntaxTreeNode, builder);
                    }
                }
            }

            return builder.ToString();
        }

        private void AddFormatToFormats(IList<List<string>> formats, IList<int> currentIndexesInFormats,
            SyntaxTreeNode syntaxTreeNode, ISeparatorConverter separatorConverter)
        {
            var tokensCount = GetTokensToFormatCount(syntaxTreeNode);
            formats.Add(separatorConverter.GetTokensFormats(syntaxTreeNode.Token.Value, tokensCount));
            currentIndexesInFormats.Add(0);
        }

        private void ApplyFormatsToNode(IList<List<string>> formats, IList<int> currentIndexesInFormats,
            SyntaxTreeNode syntaxTreeNode, StringBuilder builder)
        {
            var valueToAppend = syntaxTreeNode.Token.Value;
            for (var i = formats.Count - 1; i >= 0; i--)
            {
                valueToAppend = string.Format(formats[i][currentIndexesInFormats[i]], valueToAppend);
                currentIndexesInFormats[i]++;
            }

            builder.Append(valueToAppend);
        }

        private void DeleteFormatFromFormats(IList<List<string>> formats, IList<int> currentIndexesInFormats)
        {
            var indexToDelete = Enumerable.Range(0, formats.Count)
                .First(i => currentIndexesInFormats[i] == formats[i].Count);
            formats.RemoveAt(indexToDelete);
            currentIndexesInFormats.RemoveAt(indexToDelete);
        }

        private bool IsCorrectSeparatorInNode(SyntaxTreeNode treeNode)
        {
            return treeNode.Token.IsSeparator && treeNode.Children.Count > 0 && new SyntaxTree {Root = treeNode}
                       .Enumerate().Skip(1).Any(n => n.Token.IsSeparator && n.Token.Value == treeNode.Token.Value);
        }

        private int GetTokensToFormatCount(SyntaxTreeNode treeNode)
        {
            return new SyntaxTree {Root = treeNode}.Enumerate().Skip(1)
                .TakeWhile(n => !n.Token.IsSeparator || n.Token.Value != treeNode.Token.Value)
                .Count(n => !n.Token.IsSeparator);
        }
    }
}