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
            Tags = new Dictionary<TagType, Tag>()
            {
                {TagType.Em, new Tag("_", "_", new TagType[] { })},
                {TagType.Strong, new Tag("__", "__", new TagType[] {TagType.Em})}
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
                                   (StackOfTags.Peek().Tagtype != tag.Tagtype && !StackOfTags.Peek().IsOpen)))
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
                : TreeBuilder.ReplaceToSyntaxTree(str, ValidTags, Tags);
        }

        private TagToken CreateTag(string line, int i)
        {
            if (IsStrongTag(line, i) && IsCloseTag(line, i))
                return new TagToken(TagType.Strong, false, i);

            if (IsStrongTag(line, i) && IsOpenTag(line, i + 1))
                return new TagToken(TagType.Strong, true, i);

            if (IsEmTag(line, i) && IsCloseTag(line, i))
                return new TagToken(TagType.Em, false, i);

            if (IsEmTag(line, i) && IsOpenTag(line, i))
                return new TagToken(TagType.Em, true, i);
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

        private static bool IsOpenTag(string line, int i)
        {
            return i + 1 < line.Length && line[i + 1] != ' ' && !char.IsNumber(line, i + 1);
        }

        private static bool IsCloseTag(string line, int i)
        {
            return i > 0 && line[i - 1] != ' ' && !char.IsNumber(line, i - 1);
        }

        private static bool IsEmTag(string line, int i) =>
            line[i] == '_' && (i + 1 >= line.Length || line[i + 1] != '_');

        private static bool IsStrongTag(string line, int i)
        {
            return i < line.Length - 1 && line[i] == '_' && line[i + 1] == '_' &&
                   (i + 2 >= line.Length || line[i + 2] != '_');
        }
    }
}