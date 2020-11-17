using Markdown.TagConverters;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace Markdown
{
    public static class Md
    {
        internal const char shieldSimbol = '\\';

        public static string Render(string text)
        {
            var texts = text.Split('\n');
            var result = new StringBuilder();
            for (var i = 0; i < texts.Length; i++)
            {
                var t = texts[i];
                result.Append(RenderParagraph(t));
                if (i != texts.Length - 1)
                    result.Append('\n');
            }
            return result.ToString();
        }

        internal static StringBuilder Render(StringBuilder text) => new StringBuilder(Render(text.ToString()));
        private static string RenderParagraph(string paragraph)
        {
            var (tagInfos, result) = ProcessText(paragraph);
            var currectTags = tagInfos.GetCorrectTags();
            var text = result.ToString();
            if (!currectTags.Any())
                return text;
            var tags = new Stack<TagInfo>();
            var tagsText = new Stack<StringBuilder>();
            tagsText.Push(new StringBuilder());
            var lastCloseTagPos = 0;
            int lastTagPosition;
            TagInfo peek;
            foreach(var tag in currectTags)
            {
                peek = tags.Any() ? tags.Peek() : null;
                if(peek == null)
                {
                    ProcessTagWhenDeepZero(tag);
                    continue;
                }
                if(tag.tagConverter.StringMd == peek.tagConverter.StringMd)
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
                    lastTagPosition = Math.Max(lastCloseTagPos, peek.pos);
                    t.Append(text[lastTagPosition..text.Length]);
                    tagsText.Peek().Append(peek.Convert(t, singleTag));
                    lastCloseTagPos = text.Length;
                }
            }

            void OpenNewTag(TagInfo tag)
            {
                lastTagPosition = Math.Max(lastCloseTagPos, peek.pos);
                tagsText.Peek().Append(text[lastTagPosition..tag.pos]);
                tagsText.Push(new StringBuilder());
                tags.Push(tag);
            }

            void CloseTagAndAppendResultTextInUpperText(TagInfo tag)
            {
                tags.Pop();
                var t = tagsText.Pop();
                lastTagPosition = Math.Max(lastCloseTagPos, peek.pos);
                t.Append(text[lastTagPosition..(tag.pos + tag.tagConverter.LengthMd)]);
                tagsText.Peek().Append(peek.Convert(t, tag));
                lastCloseTagPos = tag.pos + tag.tagConverter.LengthMd;
            }

            void ProcessTagWhenDeepZero(TagInfo tag)
            {
                tags.Push(tag);
                var upperString = tagsText.Peek();
                upperString.Append(text[lastCloseTagPos..tag.pos]);
                tagsText.Push(new StringBuilder());
            }
        }

        private static (IEnumerable<TagInfo> tagInfos, StringBuilder result) ProcessText(string text)
        {
            var result = new StringBuilder();
            var tagStack = new List<TagInfo>();
            int ofset;
            string md;
            TagConverterBase tag;
            var pos = 0;
            for (var i = 0; i < text.Length; i += ofset)
            {
                if (Shield(text, i))
                {
                    ofset = 2;
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
                    ofset = md.Length;
                    pos += md.Length;
                    continue;
                }
                result.Append(text[i]);
                ofset = 1;
                pos++;
            }
            //я чиитал в C# есть такая вещь:
            //return new() { stack = tagStack, t = result };
            //анонимный класс, или как-то 
            return (tagStack, result);
        }

        

        private static bool Shield(string text, int position) =>
            position < text.Length - 1 &&
            text[position] == shieldSimbol && 
            (TagsAssociation.GetTagConverter(text[position + 1].ToString()) != null || text[position + 1] == shieldSimbol);
    }
}
