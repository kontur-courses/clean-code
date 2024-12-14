using Markdown.NodeView;
using Markdown.Token;

namespace Markdown.ParseTree;

public class MdParseTree : IParseTree<MdTokenType>
{
    private class Node
    {
        public Node(MdTokenType type,
            bool complete = false,
            bool insideWord = false,
            ReadOnlyMemory<char>? text = null,
            Node? parent = null
            )
        {
            Type = type;
            Children = new List<Node>();
            Complete = complete;
            InsideWord = insideWord;
            Text = text ?? ReadOnlyMemory<char>.Empty;
            Parent = parent;
        }

        public ReadOnlyMemory<char> Text { get; set; }
        public MdTokenType Type { get; set; }
        public bool Complete { get; set; }
        public bool InsideWord { get; set; }
        public List<Node> Children { get; set; }
        public Node? Parent { get; set; }
    }

    private readonly Node _root;
    private Node _current;
    
    public MdParseTree()
    {
        _root = new Node(MdTokenType.Document, true);
        _current = _root;
    }

    public ParseTreeNodeView<MdTokenType> CurrentToken =>
        new(_current.Text, _current.Type, _current.Children.Count == 0, _current.Complete, _current.InsideWord);

    public void OpenToken(MdTokenType tokenType, ReadOnlyMemory<char> text, bool insideWord = false)
    {
        var newNode = new Node(tokenType, false, insideWord, text, _current);
        _current.Children.Add(newNode);
        _current = newNode;
    }

    public void CloseCurrentToken(bool complete)
    {
        if (_current == _root)
            throw new InvalidOperationException("Cannot call CloseCurrentToken when on root node");
        _current.Complete = complete;
        _current = _current.Parent!;
    }

    public IEnumerable<BaseNodeView<MdTokenType>> Traverse()
    {
        return Traverse(_root);
    }

    private static IEnumerable<BaseNodeView<MdTokenType>> Traverse(Node node)
    {
        yield return new ParseTreeNodeView<MdTokenType>(
            node.Text, node.Type, node.Children.Count == 0, node.Complete, node.InsideWord);
        var childNodes = node.Children.SelectMany(Traverse).ToList();
        foreach (var childNode in childNodes)
            yield return childNode;
        if (childNodes.Count > 0)
            yield return new ViewEnd<MdTokenType>(node.Type);
    }
}