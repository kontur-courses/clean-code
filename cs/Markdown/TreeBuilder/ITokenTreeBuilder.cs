using System.Collections.Generic;
using Markdown.Data;
using Markdown.Data.Nodes;

namespace Markdown.TreeBuilder
{
    public interface ITokenTreeBuilder
    {
        TokenTreeNode BuildTree(IEnumerable<Token> tokens);
    }
}