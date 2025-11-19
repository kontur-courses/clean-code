using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;

namespace Markdown;

public class MarkdownParser
{
    private readonly ParserContext context;
    private readonly ParseSelector selector;
    public MarkdownParser(ParserContext context, ParseSelector selector)
    {
        this.context = context;
        this.selector = selector;
    }
    
    public MarkdownDocumentNode ParseTokens()
    {   
        while (context.Position < context.Tokens.Count)
        {
            var token = context.Tokens[context.Position];
            var parser = selector.GetParser(token.Type);
            parser.Parse();
            context.IncreasePosition();
        }

        var markdownDocumentNode = BuildMarkdownDocument();
        
        return markdownDocumentNode;
    }

    private MarkdownDocumentNode BuildMarkdownDocument()
    {   
        var markdownDocumentNode = new MarkdownDocumentNode(null, "");
        MarkdownNode currentNode = markdownDocumentNode;
        var stack = new Stack<MarkdownNode>();
        stack.Push(currentNode);
        foreach (var token in context.Tokens)
        {
            switch (token.Type)
            {
                case TokenType.Text or TokenType.Escape or TokenType.NewLine or TokenType.Space:
                {
                    if (token.Type is TokenType.NewLine)
                    {
                        var peeked = stack.Peek();
                        if (peeked is not MarkdownDocumentNode)
                        {
                            stack.Pop();
                            currentNode = peeked.Parent!;
                        }
                    }
                
                    var textNode = new TextNode(currentNode, token.Value);
                    currentNode.AddChild(textNode);
                    break;
                }
                case TokenType.Underscore or TokenType.WordUnderscore:
                {
                    var peeked = stack.Peek();
                    if (peeked is ItalicNode)
                    {
                        stack.Pop();
                        currentNode = peeked.Parent!;
                    }
                    else
                    {
                        var italicNode = new ItalicNode(currentNode, token.Value);
                        stack.Push(italicNode);
                        currentNode.AddChild(italicNode);
                        currentNode = italicNode;
                    }

                    break;
                }
                case TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore:
                {
                    var peeked = stack.Peek();
                    if (peeked is BoldNode)
                    {
                        stack.Pop();
                        currentNode = peeked.Parent!;
                    }
                    else
                    {
                        var boldNode = new BoldNode(currentNode, token.Value);
                        stack.Push(boldNode);
                        currentNode.AddChild(boldNode);
                        currentNode = boldNode;
                    }

                    break;
                }
                case TokenType.Exclamation:
                {
                    var imageNode = new ImageNode(currentNode, token.Value);
                    stack.Push(imageNode);
                    currentNode.AddChild(imageNode);
                    currentNode = imageNode;
                    break;
                }
                case TokenType.Hash:
                {
                    var headerNode = new HeaderNode(currentNode, token.Value);
                    currentNode.AddChild(headerNode);
                    currentNode = headerNode;
                    stack.Push(headerNode);
                    break;
                }
                case TokenType.RParenthesis:
                {
                    var imageNode = stack.Pop();
                    currentNode = imageNode.Parent!;
                    break;
                }
                case TokenType.RBracket:
                {
                    var altNode = stack.Pop();
                    currentNode = altNode.Parent!;
                    break;
                }
                case TokenType.LBracket:
                {
                    var altNode = new AltNode(currentNode, token.Value);
                    currentNode.AddChild(altNode);
                    stack.Push(altNode);
                    currentNode = altNode;
                    break;
                }
                case TokenType.LParenthesis:
                {
                    var urlNode = new UrlNode(currentNode, token.Value);
                    currentNode.AddChild(urlNode);
                    stack.Push(urlNode);
                    currentNode = urlNode;
                    break;
                }
            }
        }

        return markdownDocumentNode;
    }
}
