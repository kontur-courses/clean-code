using System.Text;
using Markdown.Data.Nodes;
using Markdown.TreeTranslator.TagTranslator;

namespace Markdown.TreeTranslator.NodeTranslator
{
    public class MarkdownNodeTranslator : INodeTranslator
    {
        private readonly ITagTranslator tagTranslator;

        public MarkdownNodeTranslator(ITagTranslator tagTranslator)
        {
            this.tagTranslator = tagTranslator;
        }

        public void Translate(RootTreeNode node, StringBuilder textBuilder)
        {
            TranslateChildren(node, textBuilder);
        }

        public void Translate(TextTreeNode node, StringBuilder textBuilder)
        {
            textBuilder.Append(node.Text);
        }

        public void Translate(TagTreeNode node, StringBuilder textBuilder)
        {
            var translation = tagTranslator.Translate(node.TagInfo);
            textBuilder.Append(node.IsRaw ? node.TagInfo.OpeningTag : translation.OpeningTranslation);
            TranslateChildren(node, textBuilder);
            textBuilder.Append(node.IsRaw ? node.TagInfo.ClosingTag : translation.ClosingTranslation);
        }

        private void TranslateChildren(TokenTreeNode node, StringBuilder textBuilder)
        {
            foreach (var child in node.Children)
                child.Translate(this, textBuilder);
        }
    }
}