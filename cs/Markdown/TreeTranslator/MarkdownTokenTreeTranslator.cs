using System.Text;
using Markdown.Data.Nodes;

namespace Markdown.TreeTranslator
{
    public class MarkdownTokenTreeTranslator : ITokenTreeTranslator
    {
        private readonly ITagTranslator tagTranslator;

        public MarkdownTokenTreeTranslator(ITagTranslator tagTranslator)
        {
            this.tagTranslator = tagTranslator;
        }

        public string Translate(TokenTreeNode tokenTree)
        {
            var textBuilder = new StringBuilder();
            foreach (var child in tokenTree.Children)
                TranslateNode(child, textBuilder);
            return textBuilder.ToString();
        }

        private void TranslateNode(TokenTreeNode currentNode, StringBuilder textBuilder)
        {
            textBuilder.Append(GetNodeText(currentNode));

            foreach (var child in currentNode.Children)
                TranslateNode(child, textBuilder);

            if (currentNode is TagTreeNode closedTagNode)
                textBuilder.Append(GetClosedTagNodeText(closedTagNode));
        }

        private string GetClosedTagNodeText(TagTreeNode closedTagNode)
        {
            return closedTagNode.IsRaw
                ? closedTagNode.TagInfo.ClosingTag
                : tagTranslator.TranslateClosingTag(closedTagNode.TagInfo.ClosingTag);
        }

        private string GetNodeText(TokenTreeNode currentNode)
        {
            switch (currentNode)
            {
                case TextTreeNode textNode:
                    return textNode.Text;
                case SpaceTreeNode _:
                    return " ";
                case TagTreeNode openedTagNode:
                    return tagTranslator.TranslateOpeningTag(openedTagNode.TagInfo.OpeningTag);
            }
            return null;
        }
    }
}