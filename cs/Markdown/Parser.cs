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

        return tokens[index];
    }

    private Token Current => Peek(0);
    
    public SyntaxNode Parse()
    {
        var stack = new Stack<SyntaxNode>();
        while (position < tokens.Length)
        {
            switch (Current.Kind)
            {
                case SyntaxKind.Text:
                    stack.Push(new TextNode(Current.Text));
                    break;
                case SyntaxKind.Whitespace:
                    stack.Push(new WhitespaceNode(Current.Text));
                    break;    
                case SyntaxKind.SingleUnderscore:
                    if (Peek(1).Kind == SyntaxKind.Text && Peek(2).Kind == SyntaxKind.SingleUnderscore)
                    {
                        var children = new List<SyntaxNode>();
                        children.Add(new OpenEmNode(Current.Text));
                        position++;
                        children.Add(new TextNode(Current.Text));
                        position++;
                        children.Add(new CloseEmNode(Current.Text));
                        stack.Push(new EmBodyNode(children));
                    }
                    else if (position == 0 || stack.Peek().Type == NodeType.WhitespaceNode)
                    {
                        stack.Push(new OpenEmNode(Current.Text));
                    }
                    else if (position == tokens.Length - 1 || Peek(1).Kind == SyntaxKind.Whitespace)
                    {
                        stack.Push(new CloseEmNode(Current.Text));
                        var children = new List<SyntaxNode>();
                        //TODO: not allow strong tag
                        while (stack.Any())
                        {
                            var child = stack.Pop();
                            children.Add(child);
                            if (child.Type == NodeType.OpenEmTag)
                            {
                                children.Reverse();//TODO: fix
                                stack.Push(new EmBodyNode(children));
                                break;
                            }
                        }
                    }
                    break;
                case SyntaxKind.DoubleUnderscore:
                    if (position == 0 || stack.Peek().Type == NodeType.WhitespaceNode)
                    {
                        stack.Push(new OpenStrongNode(Current.Text));
                    }
                    else if (position == tokens.Length - 1 || Peek(1).Kind == SyntaxKind.Whitespace)
                    {
                        stack.Push(new CloseStrongNode(Current.Text));
                        var children = new List<SyntaxNode>();
                        while (stack.Any())
                        {
                            var child = stack.Pop();
                            children.Add(child);
                            if (child.Type == NodeType.OpenStrongTag)
                            {
                                children.Reverse();
                                stack.Push(new StrongBodyNode(children));
                                break;
                            }
                        }
                    }
                    break;
            }
            position++;
        }

        return new BodyTag(NodeType.Root, stack.Reverse().ToArray());
        //return stack.Reverse().ToArray();
    }
    
}