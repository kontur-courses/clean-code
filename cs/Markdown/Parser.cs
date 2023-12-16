using System.Collections;

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

    private Token Current => Peek(0);
    private Token Previous => Peek(-1);

    public SyntaxNode Parse()
    {
        stack = new Stack<SyntaxNode>();
        openTagStack = new Stack<(SyntaxNode, int)>();
        position = 0;

        List<SyntaxNode>? children;
        while (position < tokens.Length)
        {
            switch (Current.Kind)
            {
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

                    children = new List<SyntaxNode>();
                    while (true)
                    {
                        var child = stack.Pop();
                        children.Add(child);
                        if (children is OpenHeaderNode)
                            break;
                    }

                    children.Reverse();

                    stack.Push(new HeaderBodyNode(children.TextifyInnerTags()));
                    break;
                case SyntaxKind.Hash:
                    var header = new OpenHeaderNode(Current.Text);
                    stack.Push(header);
                    openTagStack.Push((header, position));
                    break;
                case SyntaxKind.SingleUnderscore:
                    if (TryOpenBodyWith(new OpenEmNode(Current.Text)))
                        break;

                    stack.Push(new CloseEmNode(Current.Text));
                    children = new List<SyntaxNode>();
                    while (true)
                    {
                        var child = stack.Pop();
                        if (child is StrongBodyNode)
                            child = new TextNode(child.Text);
                        children.Add(child);
                        if (child is OpenEmNode)
                            break;
                    }

                    openTagStack.Pop();

                    children.Reverse();
                    children = children.TextifyInnerTags().ToList();
                    if (children.IsOnlyDigitsInInnerTextTags())
                        stack.Push(new TextNode(string.Join("", children.Select(child => child.Text))));
                    else
                        stack.Push(new EmBodyNode(children));

                    break;
                case SyntaxKind.DoubleUnderscore:
                    if (TryOpenBodyWith(new OpenStrongNode(Current.Text)))
                        break;

                    stack.Push(new CloseStrongNode(Current.Text));
                    children = new List<SyntaxNode>();
                    while (true)
                    {
                        var child = stack.Pop();
                        children.Add(child);
                        if (child is OpenStrongNode)
                            break;
                    }

                    openTagStack.Pop();

                    children.Reverse();
                    if (children.Count == 2)
                        stack.Push(new TextNode(children[0].Text + children[1].Text));
                    else if (children.IsOnlyDigitsInInnerTextTags())
                        stack.Push(new TextNode(string.Join("", children.Select(child => child.Text))));
                    else
                        stack.Push(new StrongBodyNode(children.TextifyInnerTags()));

                    break;
            }

            position++;
        }

        ResolveUnusedOpenedTags();

        return new BodyNode(stack.Reverse().TextifyTags());
    }

    public void ResolveUnusedOpenedTags()
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

            stack.Push(new HeaderBodyNode(children.TextifyInnerTags()));
        }
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
                    if (nodeTokenIndex == 0 || tokens[nodeTokenIndex - 1].Kind == SyntaxKind.Whitespace)
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

public static class Extensions
{
    public static IEnumerable<SyntaxNode> TextifyInnerTags(this IEnumerable<SyntaxNode> nodes)
    {
        yield return nodes.First();
        foreach (var node in nodes.Skip(1).Take(nodes.Count() - 2).TextifyTags())
            yield return node;
        yield return nodes.Last();
    }

    public static IEnumerable<SyntaxNode> TextifyTags(this IEnumerable<SyntaxNode> nodes)
    {
        foreach (var node in nodes)
        {
            if (node is TextNode || node is BodyNode)
                yield return node;
            else
                yield return new TextNode(node.Text);
        }
    }

    public static bool IsOnlyDigitsInInnerTextTags(this IEnumerable<SyntaxNode> nodes)
    {
        return nodes
            .Skip(1)
            .Take(nodes.Count() - 2)
            .Where(node => node is TextNode)
            .SelectMany(node => node.Text)
            .All(char.IsDigit);
    }
}