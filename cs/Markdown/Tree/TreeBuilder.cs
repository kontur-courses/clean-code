using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tree
{
    public static class TreeBuilder
    {
        public static SyntaxTree ReplaceToSyntaxTree(string line, List<TagToken> validTags,
            Dictionary<TagType, Tag> tags)
        {
            validTags.Reverse();
            var validStack = new Stack<TagToken>(validTags);
            var syntaxTree = new SyntaxTree();

            var branch = new Stack<TagZone>();

            var i = 0;
            while (validStack.Count != 0)
            {
                var tag1 = validStack.Pop();
                var tag2 = validStack.Pop();

                if (tag1.Position > i && tag2.Position > i)
                {
                    branch.Push(
                        new TagZone(new TagNode(tag1.Tagtype, new List<SyntaxNode>
                            {
                                new TextNode(line.Substring(tag1.Position + 1, tag2.Position - tag1.Position - 1))
                            }), tag1.Position, tag2.Position + tags[tag2.Tagtype].End.Length));

                    i = tag2.Position;
                }
                else
                {
                    var node = new TagNode(tag1.Tagtype);
                    var child = new List<TagZone>();
                    while (branch.Count != 0)
                    {
                        var el = branch.Pop();
                        if (el.Start >= tag1.Position && el.End <= tag2.Position)
                            child.Add(el);
                        else
                        {
                            branch.Push(el);
                            break;
                        }
                    }

                    child.Reverse();

                    branch.Push(new TagZone(node, tag1.Position, tag2.Position + tags[tag2.Tagtype].End.Length));

                    var j = 0;
                    var last = tag1.Position + tags[tag1.Tagtype].Start.Length;
                    foreach (var el in child)
                    {
                        node.Add(new TextNode(line.Substring(last, el.Start - last)));
                        last = el.End;
                        node.Add(el.TagNode);
                        j++;
                    }

                    node.Add(new TextNode(line.Substring(last, tag2.Position - last)));
                    i = tag2.Position;
                }
            }

            var branch3 = new List<TagZone>(branch);
            if (branch3[branch3.Count - 1].Start != 0)
            {
                syntaxTree.Add(new TextNode(line.Substring(0, branch3[branch3.Count - 1].Start)));
            }

            foreach (var zone in branch)
            {
                syntaxTree.Add(zone.TagNode);
            }

            if (line.Length - branch3[0].End != 0)
            {
                syntaxTree.Add(new TextNode(line.Substring(branch3[0].End, line.Length - branch3[0].End)));
            }

            return syntaxTree;
        }

        private class TagZone
        {
            public int Start { get; }
            public int End { get; }
            public TagNode TagNode { get; }

            public TagZone(TagNode tagNode, int start, int end)
            {
                TagNode = tagNode;
                Start = start;
                End = end;
            }
        }
    }
}