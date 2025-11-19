using Markdown.Enums;
using Markdown.Models.SyntaxTreeModels;

namespace Markdown.Models.SyntaxTree
{
    public class SyntaxTree : ISyntaxTree
    {
        /// <summary>
        /// Коллекция корневых узлов абстрактного синтаксического дерева
        /// </summary>
        public List<Node> Tree { get; }

        private List<Token> tokens;
        private int currentIndex;

        /// <summary>
        /// Строит абстрактное синтаксическое дерево (AST) из потока токенов.
        /// Анализирует последовательность токенов и создает иерархическую структуру узлов,
        /// отражающую семантическую структуру исходного документа.
        /// </summary>
        /// <param name="tokens">Коллекция токенов, полученная от лексического анализатора</param>
        public SyntaxTree(List<Token> tokens)
        {
            this.tokens = tokens ?? new List<Token>();
            currentIndex = 0;
            Tree = ParseDocument();
        }

        private List<Node> ParseDocument()
        {
            var documentNodes = new List<Node>();

            while (currentIndex < tokens.Count)
            {
                var node = ParseBlock();
                if (node != null)
                {
                    documentNodes.Add(node);
                }
            }

            return documentNodes;
        }

        private Node ParseBlock()
        {
            if (currentIndex >= tokens.Count)
                return null;

            var token = tokens[currentIndex];

            return token.Type switch
            {
                TokenType.Header => ParseHeader(),
                TokenType.Newline => ParseNewline(),
                _ => ParseParagraph()
            };
        }

        private Node ParseHeader()
        {
            currentIndex++;

            var headerContent = new List<Node>();
            while (CanContinueParsingMarkerContent(TokenType.Newline))
            {
                var inlineNode = ParseInline();
                if (inlineNode != null)
                {
                    headerContent.Add(inlineNode);
                }
            }

            if (HasMarkerTokenAtCurrentPosition(TokenType.Newline))
            {
                currentIndex++;
            }

            return new Node(NodeType.Header, headerContent, null);
        }

        private Node ParseParagraph()
        {
            var paragraphContent = new List<Node>();

            while (CanContinueParsingMarkerContent(TokenType.Newline))
            {
                var inlineNode = ParseInline();
                if (inlineNode != null)
                {
                    paragraphContent.Add(inlineNode);
                }
            }

            if (HasMarkerTokenAtCurrentPosition(TokenType.Newline))
            {
                currentIndex++;
            }

            return paragraphContent.Count > 0
                ? new Node(NodeType.Paragraph, paragraphContent, null)
                : null;
        }

        private Node ParseNewline()
        {
            currentIndex++;
            return null;
        }

        private Node ParseInline()
        {
            if (currentIndex >= tokens.Count)
                return null;

            var token = tokens[currentIndex];

            return token.Type switch
            {
                TokenType.BoldStart => ParseBold(),
                TokenType.ItalicsStart => ParseItalic(),
                TokenType.LinkStart => ParseLink(),
                TokenType.Text => ParseText(),
                _ => HandleUnexpectedToken()
            };
        }

        private Node ParseLink()
        {
            currentIndex++;

            var linkContent = new List<Node>();
            string url = null;
            string title = null;

            while (currentIndex < tokens.Count && tokens[currentIndex].Type != TokenType.LinkEnd)
            {
                if (tokens[currentIndex].Type == TokenType.LinkText)
                {
                    linkContent.Add(new Node(NodeType.Text, null, tokens[currentIndex].Value));
                }
                else
                {
                    var inlineNode = ParseInline();
                    if (inlineNode != null)
                    {
                        linkContent.Add(inlineNode);
                    }
                }
                currentIndex++;
            }

            if (currentIndex < tokens.Count && tokens[currentIndex].Type == TokenType.LinkEnd)
            {
                currentIndex++;
            }

            if (currentIndex < tokens.Count && tokens[currentIndex].Type == TokenType.UrlStart)
            {
                currentIndex++;

                while (currentIndex < tokens.Count && tokens[currentIndex].Type != TokenType.UrlEnd)
                {
                    if (tokens[currentIndex].Type == TokenType.Url)
                    {
                        url = tokens[currentIndex].Value;
                    }
                    else if (tokens[currentIndex].Type == TokenType.UrlTitle)
                    {
                        title = tokens[currentIndex].Value;
                    }
                    currentIndex++;
                }

                if (currentIndex < tokens.Count && tokens[currentIndex].Type == TokenType.UrlEnd)
                {
                    currentIndex++;
                }
            }

            return new Node(NodeType.Link, linkContent, url) { Title = title };
        }

        private Node ParseBold()
        {
            currentIndex++;

            var boldContent = new List<Node>();

            while (CanContinueParsingMarkerContent(TokenType.BoldEnd))
            {
                var inlineNode = ParseInline();
                if (inlineNode != null)
                {
                    boldContent.Add(inlineNode);
                }
            }

            if (HasMarkerTokenAtCurrentPosition(TokenType.BoldEnd))
            {
                currentIndex++;
            }

            return new Node(NodeType.Bold, boldContent, null);
        }

        private Node ParseItalic()
        {
            currentIndex++;

            var italicContent = new List<Node>();

            while (CanContinueParsingMarkerContent(TokenType.ItalicsEnd))
            {
                var inlineNode = ParseInline();
                if (inlineNode != null)
                {
                    italicContent.Add(inlineNode);
                }
            }

            if (HasMarkerTokenAtCurrentPosition(TokenType.ItalicsEnd))
            {
                currentIndex++;
            }

            return new Node(NodeType.Italic, italicContent, null);
        }

        private Node ParseText()
        {
            var token = tokens[currentIndex];
            currentIndex++;
            return new Node(NodeType.Text, null, token.Value);
        }

        private Node HandleUnexpectedToken()
        {
            currentIndex++;
            return null;
        }

        private bool CanContinueParsingMarkerContent(TokenType stopTokenType)
        {
            return currentIndex < tokens.Count && tokens[currentIndex].Type != stopTokenType;
        }

        private bool HasMarkerTokenAtCurrentPosition(TokenType expectedTokenType)
        {
            return currentIndex < tokens.Count && tokens[currentIndex].Type == expectedTokenType;
        }
    }
}