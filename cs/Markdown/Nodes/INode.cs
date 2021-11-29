using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public interface INode
    {
        public bool TryOpen(List<IToken> tokens, ref int parentTokenPosition);
        public void AddChild(INode child);
        public void Close();
        public bool ShouldBeClosedByNewToken(List<IToken> tokens, int anotherTokenPosition);
        public bool CannotBeClosed(List<IToken> tokens, int anotherTokenPosition);
        public bool ShouldBeClosedWhenParagraphEnds();
        public StringBuilder GetNodeBuilder();
    }
}