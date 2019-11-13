using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tree
{
    public abstract class SyntaxNode
    {
        public readonly List<SyntaxNode> ChildNode;
        
        protected SyntaxNode(List<SyntaxNode> childNode)
        {
            ChildNode = childNode;
        }

        public abstract string ConvertTo(ILanguage language);
    }
}