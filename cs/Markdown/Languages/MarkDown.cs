using System;
using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Languages
{
    public class MarkDown : ILanguage
    {
        private readonly Stack<TagToken> stackOfTags;
        private readonly List<TagToken> validTags;
        public Dictionary<TagType, Tag> Tags { get; }

        public MarkDown()
        {
            Tags = new Dictionary<TagType, Tag>()
            {
                {TagType.Em, new Tag("_", "_", new TagType[] { })},
                {TagType.Strong, new Tag("__", "__", new TagType[] {TagType.Em})}
            };
            stackOfTags = new Stack<TagToken>();
            validTags = new List<TagToken>();
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

                if (tag.IsOpen && (stackOfTags.Count == 0 || (stackOfTags.Peek().Tagtype != tag.Tagtype && !stackOfTags.Peek().IsOpen)))
                {
                    stackOfTags.Push(tag);
                }
                else if (!tag.IsOpen && stackOfTags.Count > 0 && stackOfTags.Peek().Tagtype == tag.Tagtype && stackOfTags.Peek().IsOpen)
                {
                    UpdateTags(tag);
                }

                if (tag.IsOpen)
                    i += Tags[tag.Tagtype].Start.Length;
                else
                    i += Tags[tag.Tagtype].End.Length;
            }

            return validTags.Count == 0 ? new SyntaxTree(new List<SyntaxNode>() { new TextNode(str) }) : TreeBuilder.ReplaceToSyntaxTree(str, validTags, Tags);
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

        private static bool IsOpenTag(string line, int i)
        {
            return i + 1 < line.Length && line[i + 1] != ' ' && !char.IsNumber(line, i + 1);
        }

        private static bool IsCloseTag(string line, int i)
        {
            return i > 0 && line[i - 1] != ' ' && !char.IsNumber(line, i - 1);
        }

        private static bool IsEmTag(string line, int i) => line[i] == '_' && (i + 1 >= line.Length || line[i + 1] != '_');

        private static bool IsStrongTag(string line, int i)
        {
            return i < line.Length - 1 && line[i] == '_' && line[i + 1] == '_' && (i + 2 >= line.Length || line[i + 2] != '_');
        }
    }
}