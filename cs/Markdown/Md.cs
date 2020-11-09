using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            if (text == null)
                throw new NullReferenceException("input must be null");
            var tags = new Stack<MarkdownTag>();
            var renderedText = new StringBuilder();

            for (var i = 0; i < text.Length; ++i)
            {
                renderedText.Append(text[i]);
                var symbol = text[i].ToString();

                if (!MarkdownTag.IsTag(symbol))
                    continue;
                var tag = new MarkdownTag(symbol, renderedText.Length - 1,
                    tags.Count == 0 || tags.Peek().Value != symbol);
                if (!tag.IsValidTag(text, i))
                    continue;

                tags.Push(tag);
                if (CanReplaceMarkdownTagsOnHtmlTags(text, renderedText.ToString(), tags))
                    renderedText = renderedText.ReplaceMarkdownTagsOnHtmlTags(tags.Pop());
            }
            return renderedText.ToString();
        }

        private static bool CanReplaceMarkdownTagsOnHtmlTags(string text, string renderedText, Stack<MarkdownTag> tags)
        {
            if (tags.Count < 2)
                return false;

            var lastTag = tags.Pop();
            if (lastTag.Value == tags.Peek().Value)
                return CheckOnSpaceBetweenUnderlines(text, renderedText, lastTag, tags.Peek());

            tags.Push(lastTag);
            return true;
        }

        private static bool CheckOnSpaceBetweenUnderlines(string text, string renderedText, MarkdownTag lastTag,
            MarkdownTag preLastTag)
        {
            if (lastTag.Value != "_")
                return true;
            return !(renderedText.Substring(preLastTag.Index + 1, lastTag.Index - preLastTag.Index).Contains(" ")
                     && (preLastTag.Index - 1 >= 0 && text[preLastTag.Index] != ' '
                         || lastTag.Index + 1 < text.Length && text[lastTag.Index + 1] != ' '));
        }
    }
}