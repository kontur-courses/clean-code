using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tags.Html;
using Markdown.Tags.Markdown;

namespace Markdown.Rules
{
    public static class MarkdownToHtml
    {
        public static readonly Dictionary<ITag, ITag> Rules = new()
        {
            {MarkdownTags.Heading, HtmlTags.Heading},
            {MarkdownTags.Italics, HtmlTags.Italics},
            {MarkdownTags.Bold, HtmlTags.Bold},
        };
    }
}