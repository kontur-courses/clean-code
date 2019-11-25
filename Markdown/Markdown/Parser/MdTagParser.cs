using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Builder;
using Markdown.MdTags;

namespace Markdown.Parser
{
    internal class MdTagParser: IParser<Tag>
    {

        private readonly List<Tag> tags = new List<Tag>();
        private readonly Stack<Tag> tagsStack = new Stack<Tag>();

        public List<Tag> Parse(string textToParse)
        {
            ResetParser();
            for (var offset = 0; offset < textToParse.Length; offset++)
            {
                var tag = GetTag(offset, textToParse);
                offset += tag.OpenedMdTag.Length;
                offset += tag.ContentLength - 1;
                var canClose = tagsStack.Count != 0 && tagsStack.Peek().CanClose(tag.ClosedMdTag);
                if (canClose)
                    RememberTagAsLast(tagsStack.Pop());
                if (tag.CanOpen(tagsStack, tag.Content) && !canClose)
                {
                    tagsStack.Push(tag);
                    continue;
                }
                RememberTagAsLast(canClose ? new SimpleTag((tag.ContentLength, tag.Content)) : 
                    new SimpleTag((tag.OpenedMdTag.Length + tag.ContentLength, tag.OpenedMdTag + tag.Content)));
            }
            CloseOpenedTags();
            return tags;
        }

        private void ResetParser()
        {
            tags.Clear();
            tagsStack.Clear();
        }

        private Tag GetTag(int index, string text)
        {
            var tag = string.Empty;
            for (var i = index; i < text.Length; i++)
            {
                if (Tag.AllTags.Contains(tag + text[i]))
                {
                    tag += text[i];
                    continue;
                }
                break;
            }

            return TagBuilder.BuildTag(tag, text, index + tag.Length);
        }

        private void RememberTagAsLast(Tag tag)
        {
            if (tag.Content == string.Empty || tag.Content == Environment.NewLine)
                return;
            if (tagsStack.Count != 0)
                tagsStack.Peek().NestedTags.Add(tag);
            else
                tags.Add(tag);
        }

        private void CloseOpenedTags()
        {
            tagsStack.Reverse().ToList().ForEach(tag => tag.AutoClose(tags));
        }
    }
}
