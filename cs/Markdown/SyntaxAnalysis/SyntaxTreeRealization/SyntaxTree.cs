namespace Markdown.SyntaxAnalysis.SyntaxTreeRealization
{
    public class SyntaxTree
    {
        public SyntaxTreeNode Root { get; }

        public SyntaxTree(SyntaxTreeNode root)
        {
            Root = root;
        }
    }
}