using Markdown.AbstractSyntaxTree;
using Markdown.NodeView;

namespace Markdown.SyntaxRules;

public interface ISyntaxRule<TTokenType>
{
    public INodeView<TTokenType> Apply(INodeView<TTokenType> nodeView);
}