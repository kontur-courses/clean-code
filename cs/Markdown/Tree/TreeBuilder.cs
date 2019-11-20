using System;
using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tree
{
    public static class TreeBuilder
    {
        public static SyntaxTree RenderTree<T>(string str)
            where T : ILanguage, new()
        {
            var el = new T();
            if (str == null)
                throw new ArgumentException("The string should not be null");

            var tags = FindTags(str, el);
            return tags.Count == 0
                ? new SyntaxTree(new List<SyntaxNode> {new TextNode(str)})
                : ReplaceToSyntaxTree(str, tags, el.Tags);
        }

        private static List<TagToken> FindTags(string str, ILanguage language)
        {
            var stackOfTags = new Stack<TagToken>();
            var validTags = new List<TagToken>();

            for (var i = 0; i < str.Length; i++)
            {
                var tag = CreateTag(str, i, language);
                if (tag == null)
                    continue;

                if (tag.IsOpen && (stackOfTags.Count == 0 ||
                                   (stackOfTags.Peek().Tagtype != tag.Tagtype)))
                {
                    stackOfTags.Push(tag);
                }
                else if (!tag.IsOpen && stackOfTags.Count > 0 && stackOfTags.Peek().Tagtype == tag.Tagtype &&
                         stackOfTags.Peek().IsOpen)
                {
                    UpdateTags(tag, stackOfTags, validTags);
                }

                if (tag.IsOpen)
                    i += language.Tags[tag.Tagtype].Start.Length;
                else
                    i += language.Tags[tag.Tagtype].End.Length;
            }

            return validTags;
        }

        private static TagToken CreateTag(string line, int i, ILanguage language)
        {
            foreach (var tag in language.Tags.Keys)
            {
                if (language.IsTag(line, i, language.Tags[tag].End) && language.IsCloseTag(line, i))
                    return new TagToken(tag, false, i);
                if (language.IsTag(line, i, language.Tags[tag].Start) &&
                    language.IsOpenTag(line, i, language.Tags[tag].Start))
                    return new TagToken(tag, true, i);
            }

            return null;
        }

        private static void UpdateTags(TagToken closeTag, Stack<TagToken> stackOfTags, List<TagToken> validTags)
        {
            while (stackOfTags.Count != 0)
            {
                if (stackOfTags.Peek().Tagtype == closeTag.Tagtype)
                    break;
                stackOfTags.Pop();
            }

            if (stackOfTags.Count == 0) return;

            var openTag = stackOfTags.Pop();
            validTags.Add(openTag);
            validTags.Add(closeTag);
        }

        private static SyntaxTree ReplaceToSyntaxTree(string line, List<TagToken> validTags,
            IReadOnlyDictionary<TagType, Tag> tags)
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