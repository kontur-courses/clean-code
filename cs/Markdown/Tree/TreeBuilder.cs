using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                : CreateTreeFromZones(tags, str);
        }

        private static List<TagZone> FindTags(string str, ILanguage language)
        {
            var stackOfTags = new Stack<TagToken>();
            var validTags = new List<TagZone>();

            for (var i = 0; i < str.Length; i++)
            {
                var tag = CreateTag(str, i, language);
                if (tag == null)
                    continue;

                if (IsCorrectNextOpenTag(tag, stackOfTags, language))
                {
                    stackOfTags.Push(tag);
                }
                else if (IsCorrectNextCloseTag(tag, stackOfTags))
                {
                    var tagZone = TakeTagZone(tag, stackOfTags, str, language);
                    validTags.Add(tagZone);
                }

                if (tag.IsOpen)
                    i += language.Tags[tag.Tagtype].Start.Length;
                else
                    i += language.Tags[tag.Tagtype].End.Length;
            }

            return validTags;
        }

        private static bool IsCorrectNextOpenTag(TagToken tag, Stack<TagToken> stackOfTags, ILanguage language)
        {
            return tag.IsOpen && (stackOfTags.Count == 0 ||
                                  (stackOfTags.Peek().Tagtype != tag.Tagtype && language
                                       .Tags[stackOfTags.Peek().Tagtype].Children.Contains(tag.Tagtype)));
        }

        private static bool IsCorrectNextCloseTag(TagToken tag, Stack<TagToken> stackOfTags)
        {
            return !tag.IsOpen && stackOfTags.Count > 0 && stackOfTags.Peek().Tagtype == tag.Tagtype &&
                   stackOfTags.Peek().IsOpen;
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

        private static TagZone TakeTagZone(TagToken closeTag, Stack<TagToken> stackOfTags, string line,
            ILanguage language)
        {
            while (stackOfTags.Count != 0)
            {
                if (stackOfTags.Peek().Tagtype == closeTag.Tagtype)
                    break;
                stackOfTags.Pop();
            }

            var openTag = stackOfTags.Pop();

            var startValTag = openTag.Position + language.Tags[openTag.Tagtype].Start.Length;
            var endValTag = closeTag.Position; 
            var startTag = openTag.Position;
            var endTag = closeTag.Position + language.Tags[closeTag.Tagtype].End.Length;

            return new TagZone(new TagNode(openTag.Tagtype), language.Tags[closeTag.Tagtype], startTag, endTag,
                line.Substring(startValTag, endValTag - startValTag));
        }

        private static SyntaxTree CreateTreeFromZones(List<TagZone> validTags, string str)
        {
            validTags.Reverse();
            var validStack = new Stack<TagZone>(validTags);
            var syntaxTree = new SyntaxTree();

            var branches = new Stack<TagZone>();

            var endLastZone = 0;
            while (validStack.Count != 0)
            {
                var tag = validStack.Pop();

                if (tag.Start > endLastZone)
                {
                    tag.TagNode.Add(new TextNode(tag.Value));
                    branches.Push(tag);
                }
                else
                {
                    var child = PutChildrenTagFromStack(branches, tag);
                    branches.Push(tag);

                    AddChildrenNodesInNode(tag, child);
                }

                endLastZone = tag.End;
            }

            AddChildrenNodesInNode(syntaxTree, 0, str.Length, branches.Reverse(), str);

            return syntaxTree;
        }

        private static IEnumerable<TagZone> PutChildrenTagFromStack(Stack<TagZone> stack, TagZone tagZone)
        {
            var children = new List<TagZone>();
            while (stack.Count != 0)
            {
                var el = stack.Pop();
                if (el.Start >= tagZone.Start && el.End <= tagZone.End)
                    children.Add(el);
                else
                {
                    stack.Push(el);
                    break;
                }
            }

            children.Reverse();
            return children;
        }

        private static void AddChildrenNodesInNode(TagZone node, IEnumerable<TagZone> list)
        {
            list = ShiftTags(list, -node.Start-node.Tag.Start.Length);
            AddChildrenNodesInNode(node.TagNode, 0, node.Value.Length, list, node.Value);
        }

        private static IEnumerable<TagZone> ShiftTags(IEnumerable<TagZone> enumerable, int shift)
        {
            return enumerable.Select(elem => new TagZone(elem.TagNode, elem.Tag, elem.Start + shift, elem.End + shift, elem.Value)).ToList();
        }
        
        private static void AddChildrenNodesInNode(INode node, int start, int end, IEnumerable<TagZone> list,
            string line)
        {
            var last = start;
            foreach (var el in list)
            {
                if (el.Start - last != 0)
                    node.Add(new TextNode(line.Substring(last, el.Start - last)));
                last = el.End;
                node.Add(el.TagNode);
            }

            if (end - last != 0)
                node.Add(new TextNode(line.Substring(last, end - last)));
        }


        private class TagZone
        {
            public int Start { get; }
            public int End { get; }
            public TagNode TagNode { get; }
            public string Value { get; }

            public Tag Tag { get; }

            public TagZone(TagNode tagNode, Tag tag, int start, int end, string value)
            {
                TagNode = tagNode;
                Tag = tag;
                Start = start;
                End = end;
                Value = value;
            }
        }
    }
}