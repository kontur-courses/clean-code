using Markdown.TagConverters;
using Markdown.Constants;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var texts = text.Split(TextConstants.paragraphSplit);
            var result = new StringBuilder();
            for (var i = 0; i < texts.Length; i++)
            {
                var t = texts[i];
                result.Append(RenderParagraph(t));
                if (i != texts.Length - 1)
                    result.Append(TextConstants.paragraphSplit);
            }
            return result.ToString();
        }

        internal static StringBuilder Render(StringBuilder text) => new StringBuilder(Render(text.ToString()));
        private static string RenderParagraph(string paragraph)
        {
            var (tagInfos, result) = ProcessText(paragraph);
            var correctTags = tagInfos.GetCorrectTags();
            var text = result.ToString();
            if (!correctTags.Any())
                return text;
            var tags = new Stack<TagInfo>();
            var tagsText = new Stack<StringBuilder>();
            tagsText.Push(new StringBuilder());
            var lastCloseTagPos = 0;
            int lastTagPosition;
            TagInfo peek;
            foreach(var tag in correctTags)
            {
                if(!tags.TryPeek(out peek))
                {
                    ProcessTagWhenDeepZero(tag);
                    continue;
                }
                if(tag.tagConverter.TagName == peek.tagConverter.TagName)
                {
                    CloseTagAndAppendResultTextInUpperText(tag);
                    continue;
                }
                OpenNewTag(tag);
            }
            ProcessSingleTags();
            tagsText.Peek().Append(text[lastCloseTagPos..text.Length]);
            return tagsText.Pop().ToString();

            void ProcessSingleTags()
            {
                foreach (var singleTag in tags)
                {
                    peek = tags.Peek();
                    var t = tagsText.Pop();
                    lastTagPosition = Math.Max(lastCloseTagPos, peek.Pos);
                    t.Append(text[lastTagPosition..text.Length]);
                    tagsText.Peek().Append(peek.Convert(t, singleTag));
                    lastCloseTagPos = text.Length;
                }
            }

            void OpenNewTag(TagInfo tag)
            {
                lastTagPosition = Math.Max(lastCloseTagPos, peek.Pos);
                tagsText.Peek().Append(text[lastTagPosition..tag.Pos]);
                tagsText.Push(new StringBuilder());
                tags.Push(tag);
            }

            void CloseTagAndAppendResultTextInUpperText(TagInfo tag)
            {
                tags.Pop();
                var t = tagsText.Pop();
                lastTagPosition = Math.Max(lastCloseTagPos, peek.Pos);
                t.Append(text[lastTagPosition..(tag.Pos + tag.tagConverter.TagName.Length)]);
                tagsText.Peek().Append(peek.Convert(t, tag));
                lastCloseTagPos = tag.Pos + tag.tagConverter.TagName.Length;
            }

            void ProcessTagWhenDeepZero(TagInfo tag)
            {
                tags.Push(tag);
                var upperString = tagsText.Peek();
                upperString.Append(text[lastCloseTagPos..tag.Pos]);
                tagsText.Push(new StringBuilder());
            }
        }

        private static (List<TagInfo> tagInfos, StringBuilder result) ProcessText(string text)
        {
            var result = new StringBuilder();
            var tagStack = new List<TagInfo>();
            int offset;
            string md;
            ITagConverter tag;
            var pos = 0;
            for (var i = 0; i < text.Length; i += offset)
            {
                if (Shield(text, i))
                {
                    offset = 2;
                    result.Append(text[i + 1]);
                    pos++;
                    continue;
                }
                md = TagsAssociation.GetTagMd(text, i, TagsAssociation.tags);
                if (md != null)
                {
                    tag = TagsAssociation.GetTagConverter(md);
                    if (tag.IsTag(text, i))
                    {
                        tagStack.Add(new TagInfo(tag, result, pos));
                    }
                    result.Append(md);
                    offset = md.Length;
                    pos += md.Length;
                    continue;
                }
                result.Append(text[i]);
                offset = 1;
                pos++;
            }
            return (tagStack, result);
        }

        private static bool Shield(string text, int position) =>
            position < text.Length - 1 &&
            text[position] == TextConstants.shield && 
            (TagsAssociation.GetTagConverter(text[position + 1].ToString()) != null || text[position + 1] == TextConstants.shield);
    }
}
