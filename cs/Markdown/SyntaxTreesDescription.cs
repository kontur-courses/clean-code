using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public delegate bool SyntaxTreesConverter(
        TreeConverter converter, List<SyntaxTree> children, int numberOfCurrentTree, StringBuilder builder, out int numberOfSyntaxTrees);

    public class SyntaxTreesDescription
    {
        public SyntaxTreesConverter TryConvertSyntaxTrees;

        public SyntaxTreesDescription(SyntaxTreesConverter tryConvertSyntaxTrees)
        {
            TryConvertSyntaxTrees = tryConvertSyntaxTrees;
        }
    }
}
