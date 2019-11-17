using System.Collections.Generic;
using System.Linq;
using Markdown.Languages;

namespace Markdown.Tree
{
    public static class TreeBuilder
    {
        public static SyntaxTree ReplaceToSyntaxTree(string line, List<TagToken> validTags,
            Dictionary<TagType, Tag> tags)
        {
            var syntaxTree = new SyntaxTree();
            var topBranch = new Stack<TagToken>();
            validTags.Sort((t1, t2) => t1.Position.CompareTo(t2.Position));
            var i = 0;
            foreach (var tag in validTags)
            {
                if (tag.IsOpen && (topBranch.Count == 0 ||
                                   tags[topBranch.Peek().Tagtype].Children.Contains(tag.Tagtype)))
                {
                    if (tag.Position - i != 0)
                    {
                        syntaxTree.Add(new TextNode(line.Substring(i, tag.Position)));
                    }

                    i = tag.Position + tags[tag.Tagtype].Start.Length;
                    topBranch.Push(tag);
                }

                if (!tag.IsOpen && topBranch.Peek().Tagtype == tag.Tagtype)
                {
                    var node = new TagNode(tag.Tagtype,
                        new List<SyntaxNode>()
                        {
                            new TextNode(
                                line.Substring(i, tag.Position - i))
                        });
                    i = tag.Position + tags[tag.Tagtype].End.Length;
                    topBranch.Pop();
                    syntaxTree.Add(node);
                }
            }

            if (line.Length - i != 0)
            {
                syntaxTree.Add(new TextNode(
                    line.Substring(i, line.Length - i)));
            }

            return syntaxTree;
        }
    }
}