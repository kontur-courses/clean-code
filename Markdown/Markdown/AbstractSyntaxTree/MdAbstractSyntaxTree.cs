using System.Collections.Immutable;
using Markdown.NodeView;
using Markdown.ParseTree;
using Markdown.SyntaxRules;
using Markdown.Token;

namespace Markdown.AbstractSyntaxTree;

public class MdAbstractSyntaxTree : IAbstractSyntaxTree<MdTokenType>
{
    private class Node : INodeView<MdTokenType>
    {
        public Node(
            MdTokenType type,
            ReadOnlyMemory<char>? text = null,
            Node? parent = null,
            bool insideWord = false)
        {
            Type = type;
            Text = text ?? ReadOnlyMemory<char>.Empty;
            Parent = parent;
            Children = new List<INodeView<MdTokenType>>();
            InsideWord = insideWord;
        }
        
        public ReadOnlyMemory<char> Text { get; set; }
        public MdTokenType Type { get; set; }
        public bool InsideWord { get; set; }
        public List<INodeView<MdTokenType>> Children { get; set; }
        public INodeView<MdTokenType>? Parent { get; set; }
    }

    private Node _root;
    private Node _current;

    private ImmutableList<ISyntaxRule<MdTokenType>> _rules;
    
    private MdAbstractSyntaxTree()
    {
        _root = new Node(MdTokenType.Document);
        _current = _root;
        _rules = ImmutableList<ISyntaxRule<MdTokenType>>.Empty;
    }
    
    public static MdAbstractSyntaxTree FromParseTree(IParseTree<MdTokenType> parseTree)
    {
        var syntaxTree = new MdAbstractSyntaxTree();
        foreach (var baseView in parseTree.Traverse())
        {
            if (baseView is ParseTreeNodeView<MdTokenType> nodeView)
            {
                if (nodeView.TokenType != MdTokenType.Document)
                {
                    if (nodeView.Complete)
                    {
                        var newNode = new Node(
                            nodeView.TokenType, nodeView.Text, syntaxTree._current, nodeView.insideWord);
                        if (nodeView.TokenType != MdTokenType.PlainText)
                            syntaxTree.AddNode(newNode);
                        else
                            syntaxTree._current.Children.Add(newNode);
                    }
                    else
                    {
                        var newNode = new Node(MdTokenType.PlainText, nodeView.Text, syntaxTree._current);
                        syntaxTree._current.Children.Add(newNode);
                    }
                }
            }
            else if (baseView is ViewEnd<MdTokenType>)
                syntaxTree.EndCurrentNode();
            else
                throw new InvalidOperationException("Unexpected node type");
        }
        return syntaxTree;
    }

    private void AddNode(Node node)
    {
        _current.Children.Add(node);
        _current = node;
    }

    private void EndCurrentNode()
    {
        if (_current != _root)
            _current = (Node) _current.Parent!;
    }
    
    public IEnumerable<BaseNodeView<MdTokenType>> Traverse()
    {
        return Traverse(_root);
    }
    
    private static IEnumerable<BaseNodeView<MdTokenType>> Traverse(INodeView<MdTokenType> node)
    {
        yield return new AbstractSyntaxTreeNodeView<MdTokenType>(node.Text, node.Type);
        var childNodes = node.Children.SelectMany(Traverse).ToList();
        foreach (var childNode in childNodes)
            yield return childNode;
        if (childNodes.Count > 0)
            yield return new ViewEnd<MdTokenType>(node.Type);
    }

    public IAbstractSyntaxTree<MdTokenType> AddRule(ISyntaxRule<MdTokenType> rule)
    {
        _rules = _rules.Add(rule);
        return this;
    }

    public IAbstractSyntaxTree<MdTokenType> ApplyRules()
    {
        INodeView<MdTokenType> syntaxTree = _root;
        syntaxTree = _rules
            .Aggregate(syntaxTree,
                (current, rule) => rule.Apply(current));
        _root = (Node) syntaxTree;
        _current = _root;
        _rules = ImmutableList<ISyntaxRule<MdTokenType>>.Empty;
        return this;
    }
}