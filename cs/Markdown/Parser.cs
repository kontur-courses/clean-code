using System.Collections;
using Markdown.Link;
using Markdown.LinkSource;
using Markdown.LinkText;

namespace Markdown;

public class Parser
{
    private readonly Token[] tokens;
    private int position;

    private Stack<SyntaxNode> stack;
    private Stack<(SyntaxNode, int)> openTagStack;

    public Parser(Token[] tokens)
    {
        this.tokens = tokens;
    }

    private Token Peek(int offset)
    {
        var index = position + offset;
        if (index >= tokens.Length)
            return tokens[tokens.Length - 1];
        if (index < 0)
            return tokens[0];

        return tokens[index];
    }

    private Token Next => Peek(1);
    private Token Current => Peek(0);
    private Token Previous => Peek(-1);

    public RootNode Parse()
    {
        stack = new Stack<SyntaxNode>();
        openTagStack = new Stack<(SyntaxNode, int)>();
        position = 0;

        List<SyntaxNode>? children;
        while (position < tokens.Length)
        {
            switch (Current.Kind)
            {
                case SyntaxKind.Unrecognized:
                case SyntaxKind.Text:
                    stack.Push(new TextNode(Current.Text));
                    break;
                case SyntaxKind.Whitespace:
                    CloseUnusedTags();
                    stack.Push(new TextNode(Current.Text));
                    break;
                case SyntaxKind.NewLine:
                    if (!openTagStack.Any(pair => pair.Item1 is OpenHeaderNode))
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    openTagStack.Clear();
                    stack.Push(new CloseHeaderNode(Current.Text));
                    children = PopStackUntilFind<OpenHeaderNode>().ToList();
                    stack.Push(new HeaderTaggedBodyNode(children));
                    break;
                case SyntaxKind.Hash:
                    if (Next.Kind != SyntaxKind.Whitespace)
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    var header = new OpenHeaderNode(Current.Text);
                    stack.Push(header);
                    openTagStack.Push((header, position));
                    break;
                case SyntaxKind.OpenSquareBracket:
                    var openLinkTextNode = new OpenLinkTextNode(Current.Text);
                    stack.Push(openLinkTextNode);
                    openTagStack.Push((openLinkTextNode, position));
                    break;
                case SyntaxKind.CloseSquareBracket:
                    if (openTagStack.Peek().Item1 is not OpenLinkTextNode)
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    openTagStack.Pop();

                    stack.Push(new CloseLinkTextNode(Current.Text));
                    children = PopStackUntilFind<OpenLinkTextNode>().ToList();
                    stack.Push(new LinkTextTaggedBody(children));
                    break;
                case SyntaxKind.OpenRoundBracket:
                    if (stack.Peek() is not LinkTextTaggedBody)
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    var openLinkSourceNode = new OpenLinkSourceNode(Current.Text);
                    stack.Push(openLinkSourceNode);
                    openTagStack.Push((openLinkSourceNode, position));
                    break;
                case SyntaxKind.CloseRoundBracket:
                    if (openTagStack.Peek().Item1 is not OpenLinkSourceNode)
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    openTagStack.Pop();

                    stack.Push(new CloseLinkSourceNode(Current.Text));
                    children = PopStackUntilFind<OpenLinkSourceNode>().ToList();
                    var linkSourceNode = new LinkSourceTaggedBody(children);

                    if (stack.Peek() is LinkTextTaggedBody)
                        stack.Push(new LinkNode(new[] { stack.Pop(), linkSourceNode }));
                    else
                        stack.Push(new TextNode(linkSourceNode.Text));
                    break;
                case SyntaxKind.SingleUnderscore:
                    if (TryOpenBodyWith(new OpenEmNode(Current.Text)))
                        break;

                    stack.Push(new CloseEmNode(Current.Text));
                    children = PopStackUntilFind<OpenEmNode>()
                        .Select(child => child is StrongTaggedBodyNode ? new TextNode(child.Text) : child)
                        .ToList();
                    openTagStack.Pop();

                    if (children.IsOnlyDigitsInInnerTextTags())
                        stack.Push(new TextNode(string.Join("", children.Select(child => child.Text))));
                    else
                        stack.Push(new EmTaggedBodyNode(children));

                    break;
                case SyntaxKind.DoubleUnderscore:
                    if (TryOpenBodyWith(new OpenStrongNode(Current.Text)))
                        break;

                    stack.Push(new CloseStrongNode(Current.Text));
                    children = PopStackUntilFind<OpenStrongNode>().ToList();
                    openTagStack.Pop();

                    if (children.Count == 2)
                        stack.Push(new TextNode(children[0].Text + children[1].Text));
                    else if (children.IsOnlyDigitsInInnerTextTags())
                        stack.Push(new TextNode(string.Join("", children.Select(child => child.Text))));
                    else
                        stack.Push(new StrongTaggedBodyNode(children));

                    break;
                default:
                    throw new Exception("Unhandled token");
            }

            position++;
        }

        ResolveUnusedOpenedTags();

        return new RootNode(stack.Reverse().TextifyTags());
    }

    private void ResolveUnusedOpenedTags()
    {
        if (openTagStack.Any(pair => pair.Item1 is OpenHeaderNode))
        {
            var children = new List<SyntaxNode>() { new CloseHeaderNode("") };
            while (true)
            {
                var child = stack.Pop();
                children.Add(child);
                if (child is OpenHeaderNode)
                    break;
            }

            children.Reverse();

            stack.Push(new HeaderTaggedBodyNode(children));
        }
    }

    private IEnumerable<SyntaxNode> PopStackUntilFind<TNode>()
        where TNode : SyntaxNode
    {
        var list = new List<SyntaxNode>();
        while (true)
        {
            var child = stack.Pop();
            list.Add(child);
            if (child is TNode)
                break;
        }

        list.Reverse();
        return list.TextifyInnerTags();
    }

    private void CloseUnusedTags()
    {
        var unclosedTags = new Stack<(SyntaxNode, int)>();
        while (openTagStack.Count > 0)
        {
            var (node, nodeTokenIndex) = openTagStack.Pop();
            switch (node)
            {
                case OpenEmNode:
                case OpenStrongNode:
                    if ((nodeTokenIndex == 0 || tokens[nodeTokenIndex - 1].Kind == SyntaxKind.Whitespace) &&
                        tokens[nodeTokenIndex + 1].Kind != SyntaxKind.Whitespace)
                        unclosedTags.Push((node, nodeTokenIndex));
                    break;
                default:
                    unclosedTags.Push((node, nodeTokenIndex));
                    break;
            }
        }

        foreach (var tag in unclosedTags)
            openTagStack.Push(tag);
    }

    private bool TryOpenBodyWith(SimpleNode node)
    {
        if (openTagStack.All(pair => pair.Item1.GetType() != node.GetType()))
        {
            stack.Push(node);
            openTagStack.Push((node, position));
            return true;
        }

        if (openTagStack.Peek().Item1.GetType() != node.GetType())
        {
            var deleted = openTagStack.Pop().Item1;
            stack.Push(new TextNode(Current.Text));
            while (deleted.GetType() != node.GetType())
                deleted = openTagStack.Pop().Item1;
            return true;
        }

        if (Previous.Kind == SyntaxKind.Whitespace)
        {
            stack.Push(new TextNode(Current.Text));
            return true;
        }

        return false;
    }
}