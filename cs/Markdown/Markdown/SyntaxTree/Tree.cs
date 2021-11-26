using System.Collections.Generic;

namespace Markdown.SyntaxTree
{
    public class Tree
    {
        public MdTags.Tag Root;
        public readonly List<Tree> Children = new List<Tree>();

        public Tree(MdTags.Tag root)
        {
            Root = root;
        }
    }
}