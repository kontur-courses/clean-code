using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TreeConverter
    {
        private readonly List<SyntaxTreesDescription> syntaxTreesDescriptions;
        public readonly SyntaxTree RootTree;

        public TreeConverter(List<SyntaxTreesDescription> syntaxTreesDescriptions, SyntaxTree rootTree)
        {
            this.syntaxTreesDescriptions = syntaxTreesDescriptions;
            RootTree = rootTree;
        }

        public string GetTaggedText()
        {
            var builder = new StringBuilder();
            AddTreeText(RootTree, builder);
            return builder.ToString();
        }

        public void AddTreeText(SyntaxTree tree, StringBuilder builder)
        {
            if (!tree.IsHasChildren)
            {
                builder.Append(Token.ConcatenateTokens(tree.Tokens));
                return;
            }

            var numberOfCurrentTree = 0;
            while (numberOfCurrentTree < tree.Children.Count)
            {
                var isTextAdded = false;
                foreach (var syntaxTreesDescription in syntaxTreesDescriptions)
                {
                    if (syntaxTreesDescription.TryConvertSyntaxTrees(
                        this, tree.Children, numberOfCurrentTree, builder,
                        out var numberOfSyntaxTrees))
                    {
                        numberOfCurrentTree += numberOfSyntaxTrees;
                        isTextAdded = true;
                        break;
                    }
                }
                if (!isTextAdded)
                    throw new Exception("Correct syntax trees description not found");
            }
        }
    }
}
