using System.Net.Mime;

namespace Markdown;

public class Parser
{
    private readonly Token[] tokens;
    private int position;

    public Parser(Token[] tokens)
    {
        this.tokens = tokens;
        position = 0;
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
        var stack = new Stack<SyntaxNode>();
        int openEmTagIndex = -1, openStrongTagIndex = -1;
        //TODO: stack of opened tags?
        while (position < tokens.Length)
        {
            List<SyntaxNode>? children;
            switch (Current.Kind)
            {
                case SyntaxKind.Text:
                    stack.Push(new TextNode(Current.Text));
                    break;
                case SyntaxKind.Whitespace:
                    if (openEmTagIndex != -1 && openEmTagIndex != 0)
                    {
                        if (tokens[openEmTagIndex - 1].Kind != SyntaxKind.Whitespace)
                            openEmTagIndex = -1;
                    }

                    if (openStrongTagIndex != -1 && openStrongTagIndex != 0)
                    {
                        if (tokens[openStrongTagIndex - 1].Kind != SyntaxKind.Whitespace)
                            openStrongTagIndex = -1;
                    }

                    stack.Push(new TextNode(Current.Text));
                    break;
                case SyntaxKind.SingleUnderscore:
                    if (openEmTagIndex == -1)
                    {
                        openEmTagIndex = position;
                        stack.Push(new OpenEmNode(Current.Text));
                        break;
                    }

                    if (Previous.Kind == SyntaxKind.Whitespace)
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    if (openStrongTagIndex != -1 && openEmTagIndex < openStrongTagIndex)
                    {
                        stack.Push(new TextNode(Current.Text));
                        openStrongTagIndex = -1;
                        openEmTagIndex = -1;
                        break;
                    }

                    stack.Push(new CloseEmNode(Current.Text));
                    children = new List<SyntaxNode>();
                    while (true)
                    {
                        var child = stack.Pop();
                        children.Add(child);
                        if (child.Type == NodeType.OpenEmTag)
                            break;
                    }

                    children.Reverse();
                    if (children
                        .Where(child => child.Type != NodeType.OpenEmTag && child.Type != NodeType.CloseEmTag)
                        .SelectMany(child => child.Text)
                        .All(char.IsDigit)
                       )
                    {
                        stack.Push(new TextNode(string.Join("", children.Select(child => child.Text))));
                    }
                    else
                    {
                        //TODO: textify
                        stack.Push(new EmBodyNode(children));
                    }

                    openEmTagIndex = -1;
                    break;
                case SyntaxKind.DoubleUnderscore:
                    if (openStrongTagIndex == -1)
                    {
                        openStrongTagIndex = position;
                        stack.Push(new OpenStrongNode(Current.Text));
                        break;
                    }

                    if (Previous.Kind == SyntaxKind.Whitespace)
                    {
                        stack.Push(new TextNode(Current.Text));
                        break;
                    }

                    if (openStrongTagIndex != -1 && openStrongTagIndex < openEmTagIndex)
                    {
                        stack.Push(new TextNode(Current.Text));
                        openStrongTagIndex = -1;
                        openEmTagIndex = -1;
                        break;
                    }

                    stack.Push(new CloseStrongNode(Current.Text));
                    children = new List<SyntaxNode>();
                    while (true)
                    {
                        var child = stack.Pop();
                        children.Add(child);
                        if (child.Type == NodeType.OpenStrongTag)
                            break;
                    }

                    children.Reverse();
                    if (children.Count == 2)
                        stack.Push(new TextNode(children[0].Text + children[1].Text));
                    else if (openEmTagIndex != -1 && openEmTagIndex < openStrongTagIndex)
                    {
                        stack.Push(new TextNode(string.Join("", children.Select(child => child.Text))));
                    }
                    else
                        //TODO: textify
                        stack.Push(new StrongBodyNode(children));

                    openStrongTagIndex = -1;
                    break;
            }

            position++;
        }

        return new BodyTag(NodeType.Root, stack.Select(node =>
            {
                if (node is TextNode || node is WhitespaceNode || node is BodyTag)
                    return node;
                return new TextNode(node.Text);
            })
            .Reverse()
            .ToArray()
        );
        //return stack.Reverse().ToArray();
    }
}