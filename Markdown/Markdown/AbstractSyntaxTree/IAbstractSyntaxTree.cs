using Markdown.NodeView;
using Markdown.SyntaxRules;
using Markdown.Traversable;

namespace Markdown.AbstractSyntaxTree;

public interface IAbstractSyntaxTree<TTokenType> : ITraversable<BaseNodeView<TTokenType>>
{
    public IAbstractSyntaxTree<TTokenType> AddRule(ISyntaxRule<TTokenType> rule);
    public IAbstractSyntaxTree<TTokenType> ApplyRules();
}