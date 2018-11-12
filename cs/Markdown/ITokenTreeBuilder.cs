using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenTreeBuilder
    {
        TokenTreeNode BuildTree(IEnumerable<string> tokens);
    }
}