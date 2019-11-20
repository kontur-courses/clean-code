using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.MdTags;

namespace Markdown.Parser
{
    internal class MdTagParser: IParser<Tag>
    {
        private readonly Dictionary<string, Func<Tag>> mdTags = new Dictionary<string, Func<Tag>>()
        {
            { "__", () => new StrongTag() },
            { "_", () => new EmTag() },
            { "**", () => new StrongTag() },
            { "*", () => new ListTag() },
            { "~", () => new StrikeTag() },
            { "`", () => new CodeTag() },
            { "", () => new SimpleTag() },
            { "#", () => new HeaderTag("#")},
            { "##", () => new HeaderTag("##") },
            { "###", () => new HeaderTag("###") },
            { "####", () => new HeaderTag("####") },
            { "#####", () => new HeaderTag("#####") },
            { "######", () => new HeaderTag("######") },
            { "***", () => new HorizontalTag() },
            { "___", () => new HorizontalTag() },
            { ">", () => new BlockquoteTag() }
        };
        private readonly List<Tag> tags = new List<Tag>();
        private readonly Stack<Tag> tagsStack = new Stack<Tag>();

        public List<Tag> Parse(string textToParse)
        {
            ResetParser();
            for (var i = 0; i < textToParse.Length; i++)
            {
                var tagInfo = GetTag(i, textToParse);
                i += tagInfo.Length;
                var tag = mdTags[tagInfo]();
                var (length, content) = tag.GetContent(i, textToParse);
                i += length - 1;
                var canClose = tagsStack.Count != 0 && tagsStack.Peek().CanClose(tag.ClosedMdTag);
                if (canClose) SaveIntoLastTag(tagsStack.Pop());
                if (tag.CanOpen(tagsStack, content) && !canClose)
                {
                    OpenTag(content, tag);
                    continue;
                }
                SaveIntoLastTag(canClose ? new SimpleTag(content) : new SimpleTag(tagInfo + content));
            }
            CloseOpenedTags();
            return tags;
        }

        private void ResetParser()
        {
            tags.Clear();
            tagsStack.Clear();
        }

        private string GetTag(int index, string text)
        {
            var tag = string.Empty;
            for (var i = index; i < text.Length; i++)
            {
                if (!mdTags.ContainsKey(tag + text[i])) break;
                tag += text[i];
            }
            return tag;
        }

        private void SaveIntoLastTag(Tag tag)
        {
            if (tag.Content == string.Empty || tag.Content == "\n" || tag.Content == "\t") return;
            if (tagsStack.Count != 0) tagsStack.Peek().NestedTags.Add(tag);
            else tags.Add(tag);
        }

        private void CloseOpenedTags()
        {
            tagsStack.Reverse().ToList().ForEach(tag => tag.AutoClose(tags));
        }

        private void OpenTag(string content, Tag tag)
        {
            tag.NestedTags.Add(new SimpleTag(content));
            tagsStack.Push(tag);
        }
    }
}
