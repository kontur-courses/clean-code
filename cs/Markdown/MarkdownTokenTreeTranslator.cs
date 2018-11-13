using System.Text;

namespace Markdown
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
            var text = "";
            switch (currentNode.Type)
            {
                case TokenType.Text:
                    text = currentNode.Text;
                    break;
                case TokenType.Space:
                    text = " ";
                    break;
                case TokenType.Tag:
                    text = tagTranslator.TranslateOpeningTag(currentNode.Text);
                    break;
                case TokenType.EscapeSymbol:
                    text = "\\";
                    break;
            }
            textBuilder.Append(text);

            foreach (var child in currentNode.Children)
                TranslateNode(child, textBuilder);

            if (currentNode.Type != TokenType.Tag)
                return;
            text = currentNode.IsRaw ? currentNode.Text : tagTranslator.TranslateClosingTag(currentNode.Text);
            textBuilder.Append(text);
        }
    }
}