using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class TreeConverter
    {
        private readonly Dictionary<SyntaxTreeType, Tuple<string, string>> tagsForTokenTypes;
        private readonly Func<SyntaxTree, int, bool> ruleForTagsAdding;

        public TreeConverter(Dictionary<SyntaxTreeType, Tuple<string, string>> tagsForTokenTypes, 
            Func<SyntaxTree, int, bool> ruleForTagsAdding)
        {
            this.tagsForTokenTypes = tagsForTokenTypes;
            this.ruleForTagsAdding = ruleForTagsAdding;
        }

        public string GetTaggedText(SyntaxTree tree, int tokenIndex)
        {
            var isNeedToAddTags = (tagsForTokenTypes.ContainsKey(tree.Type)) && ruleForTagsAdding(tree, tokenIndex);
            var builder = new StringBuilder();
            builder.Append(isNeedToAddTags ? tagsForTokenTypes[tree.Type].Item1 : tree.StartLine);
            if (!tree.IsHasChildren)
                builder.Append(Token.ConcatenateTokens(tree.Tokens));
            else
                AddChildTexts(tree, builder);
            builder.Append(isNeedToAddTags ? tagsForTokenTypes[tree.Type].Item2 : tree.EndLine);
            return builder.ToString();
        }

        private void AddChildTexts(SyntaxTree tree, StringBuilder builder)
        {
            var currentIndex = 0;
            foreach (var childTree in tree.Children)
            {
                if (childTree.StartLine.Length > 0)
                    currentIndex++;
                var treeLine = GetTaggedText(childTree, currentIndex);
                builder.Append(treeLine);
                currentIndex += childTree.Tokens.Count;
                if (childTree.EndLine.Length > 0)
                    currentIndex++;
            }
        }
    }
}
