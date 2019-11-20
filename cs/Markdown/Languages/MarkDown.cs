using System;
using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Languages
{
    public class MarkDown : ILanguage
    {
        private Stack<TagToken> StackOfTags { get; }
        private List<TagToken> ValidTags { get; }
        public Dictionary<TagType, Tag> Tags { get; }

        public MarkDown()
        {
            Tags = new Dictionary<TagType, Tag>
            {
                {TagType.Strong, new Tag("__", "__", new TagType[] {TagType.Em})},
                {TagType.Em, new Tag("_", "_", new TagType[] { })}
            };
            StackOfTags = new Stack<TagToken>();
            ValidTags = new List<TagToken>();
        }

        public SyntaxTree RenderTree(string str)
        {
            if (str == null)
                throw new ArgumentException("The string should not be null");
            for (var i = 0; i < str.Length; i++)
            {
                var tag = CreateTag(str, i);
                if (tag == null)
                    continue;

                if (tag.IsOpen && (StackOfTags.Count == 0 ||
                                   (StackOfTags.Peek().Tagtype != tag.Tagtype)))
                {
                    StackOfTags.Push(tag);
                }
                else if (!tag.IsOpen && StackOfTags.Count > 0 && StackOfTags.Peek().Tagtype == tag.Tagtype &&
                         StackOfTags.Peek().IsOpen)
                {
                    UpdateTags(tag);
                }

                if (tag.IsOpen)
                    i += Tags[tag.Tagtype].Start.Length;
                else
                    i += Tags[tag.Tagtype].End.Length;
            }

            return ValidTags.Count == 0
                ? new SyntaxTree(new List<SyntaxNode>() {new TextNode(str)})
                : TreeBuilder.ReplaceToSyntaxTree(str, ValidTags, new Dictionary<TagType, Tag>(Tags));
        }

        private TagToken CreateTag(string line, int i)
        {
            foreach (var tag in Tags.Keys)
            {
                if (IsTag(line, i, Tags[tag].End) && IsCloseTag(line, i))
                    return new TagToken(tag, false, i);
                if (IsTag(line, i, Tags[tag].Start) && IsOpenTag(line, i, Tags[tag].Start))
                    return new TagToken(tag, true, i);
            }

            return null;
        }

        private void UpdateTags(TagToken closeTag)
        {
            while (StackOfTags.Count != 0)
            {
                if (StackOfTags.Peek().Tagtype == closeTag.Tagtype)
                    break;
                StackOfTags.Pop();
            }

            if (StackOfTags.Count == 0) return;

            var openTag = StackOfTags.Pop();
            ValidTags.Add(openTag);
            ValidTags.Add(closeTag);
        }

        private static bool IsOpenTag(string line, int i, string tag)
        {
            return i + tag.Length < line.Length && line[i + tag.Length] != ' ' && !char.IsNumber(line, i + tag.Length);
        }

        private static bool IsCloseTag(string line, int i)
        {
            return i > 0 && line[i - 1] != ' ' && !char.IsNumber(line, i - 1);
        }

        private bool IsTag(string line, int i, string tag)
        {
            return i + tag.Length <= line.Length && tag == line.Substring(i, tag.Length)
                                                 && (i + tag.Length >= line.Length ||
                                                     line[i + tag.Length] != tag[tag.Length - 1]);
        }
    }
}