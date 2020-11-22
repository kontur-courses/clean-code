using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var inUnorderedList = false;
            var renderedParagraphs = new List<string>();
            foreach (var paragraph in text.Split(Environment.NewLine))
            {
                var tags = TagParser.GetTagsFromParagraph(paragraph);
                if (tags.IsNeedToAddUnorderedListTag(inUnorderedList))
                {
                    inUnorderedList = !inUnorderedList;
                    var renderedUnorderedListTag = RenderParagraph(
                        string.Empty, new List<Tag> {UnorderedListTagHelper.GetTag(0, inUnorderedList)});
                    renderedParagraphs.Add(renderedUnorderedListTag);
                }

                renderedParagraphs.Add(RenderParagraph(paragraph, tags));
            }

            if (inUnorderedList)
            {
                var renderedUnorderedListTag = RenderParagraph(
                    string.Empty, new List<Tag> {UnorderedListTagHelper.GetTag(0, false)});
                renderedParagraphs.Add(renderedUnorderedListTag);
            }

            return string.Join(Environment.NewLine, renderedParagraphs);
        }

        private static string RenderParagraph(string paragraph, IEnumerable<Tag> tags)
        {
            return MarkdownToHtmlConverter.Convert(paragraph, tags);
        }
    }
}