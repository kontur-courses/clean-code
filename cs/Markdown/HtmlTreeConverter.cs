using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    static class HtmlTreeConverter
    {
        public static bool TryAddSurroundingTags(
            TreeConverter converter, List<SyntaxTree> children, int numberOfCurrentTree, StringBuilder builder,
            out int numberOfSyntaxTrees)
        {
            numberOfSyntaxTrees = 1;
            var tree = children[numberOfCurrentTree];
            var isNeedToAddTags = Md.TagsForTokenTypes.ContainsKey(tree.Type) && 
                                  RuleForTagsAdding.IsNeedToAddTags(tree, converter.RootTree.Tokens);
            if (!isNeedToAddTags)
                return false;
            builder.Append(Md.TagsForTokenTypes[tree.Type].Item1);
            converter.AddTreeText(tree, builder);
            builder.Append(Md.TagsForTokenTypes[tree.Type].Item2);
            return true;
        }

        public static bool TryAddLinkTag(
            TreeConverter converter, List<SyntaxTree> children, int numberOfCurrentTree, StringBuilder builder, 
            out int numberOfSyntaxTrees)
        {
            numberOfSyntaxTrees = 2;
            if (numberOfCurrentTree >= children.Count - 1)
                return false;
            if (children[numberOfCurrentTree].Type != SyntaxTreeType.TextInSquareBrackets ||
                children[numberOfCurrentTree + 1].Type != SyntaxTreeType.TextInParentheses)
                return false;
            var link = Token.ConcatenateTokens(children[numberOfCurrentTree + 1].Tokens);
            var linkOpeningTag = $"<a href=\"{link}\">";
            builder.Append(linkOpeningTag);
            converter.AddTreeText(children[numberOfCurrentTree], builder);
            builder.Append("</a>");
            return true;
        }

        public static bool AddDefaultText(TreeConverter converter, List<SyntaxTree> children, int numberOfCurrentTree, StringBuilder builder, 
            out int numberOfSyntaxTrees)
        {
            numberOfSyntaxTrees = 1;
            var tree = children[numberOfCurrentTree];
            builder.Append(tree.StartLine);
            converter.AddTreeText(tree, builder);
            builder.Append(tree.EndLine);
            return true;
        }
    }
}
