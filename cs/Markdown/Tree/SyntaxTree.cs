using System.Collections.Generic;
using System.Text;
using Markdown.Languages;

namespace Markdown.Tree
{
    public class SyntaxTree:SyntaxNode
    {
        public SyntaxTree(List<SyntaxNode> childNode) : base(childNode) { }
        
        public SyntaxTree() : base(new List<SyntaxNode>()) { }
        
        public void Add(SyntaxNode node)
        {
            ChildNode.Add(node);
        }

        public override string ConvertTo(ILanguage language)
        {
            var result = new StringBuilder();
            foreach (var child in ChildNode)
            {
                result.Append(child.ConvertTo(language));
            }
            return result.ToString();
        }
    }
}