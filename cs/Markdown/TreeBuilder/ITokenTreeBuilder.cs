using System.Collections.Generic;
using Markdown.Data.Nodes;

namespace Markdown.TreeBuilder
{
    public interface ITokenTreeBuilder
    {
        TokenTreeNode BuildTree(IEnumerable<string> tokens);
    }
}