using Markdown.NodeView;
using Markdown.Token;

namespace Markdown.SyntaxRules;

public abstract class MdValidationRule : ISyntaxRule<MdTokenType>
{
    protected abstract bool CheckNode(INodeView<MdTokenType> currentNode, INodeView<MdTokenType> parentNode);
    
    public INodeView<MdTokenType> Apply(INodeView<MdTokenType> nodeView)
    {
        for (var i = 0; i < nodeView.Children.Count; i++)
        {
            var childNode = nodeView.Children[i];
            if (CheckNode(childNode, nodeView))
            {
                childNode.Type = MdTokenType.PlainText;
                foreach (var toMove in childNode.Children.AsEnumerable().Reverse())
                {
                    nodeView.Children.Insert(i + 1, toMove);
                    toMove.Parent = nodeView;
                }
                
                nodeView.Children.Insert(i + 1 + childNode.Children.Count, childNode);
                childNode.Children.Clear();
            }
            
            Apply(childNode);
        }
        
        return nodeView;
    }
}