using System.Collections.Generic;
using Markdown.Tokenization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeRealization
{
    public class SyntaxTreeNode
    {
        public Token Token { get; }
        private readonly List<SyntaxTreeNode> children;

        public IReadOnlyList<SyntaxTreeNode> Children => children.AsReadOnly();

        public SyntaxTreeNode(Token token)
        {
            Token = token;
            children = new List<SyntaxTreeNode>();
        }

        public void AddChild(SyntaxTreeNode child)
        {
            children.Add(child);
        }
    }
}