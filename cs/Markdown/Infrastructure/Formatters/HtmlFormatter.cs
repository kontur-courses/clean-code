using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Formatters
{
    public class HtmlFormatter : BlockFormatter
    {
        public HtmlFormatter()
        {
            GeneralWrappers = new Dictionary<Style, Func<IEnumerable<string>, IEnumerable<string>>>
            {
                {Style.None, Wrap("", "")},
                {Style.Bold, Wrap("<strong>", "</strong>")},
                {Style.Angled, Wrap("<em>", "</em>")},
                {Style.Header, Wrap("<h1>", "</h1>")}
            };
        }

        private Func<IEnumerable<string>, IEnumerable<string>> GetPictureWrapper(string description)
        {
            return Wrap("<img src=\"", $"\" alt=\"{description}\">");
        }

        private Func<IEnumerable<string>, IEnumerable<string>> GetLinkWrapper(string link)
        {
            return Wrap("<a href=\"", $"\">{link}</a>");
        }

        public override IEnumerable<string> Format(Tag tag, IEnumerable<string> words)
        {
            if (GeneralWrappers.TryGetValue(tag.Style, out var wrap))
                return wrap(words);

            return tag switch
            {
                PictureTag pictureTag => GetPictureWrapper(pictureTag.Description)(words),
                LinkTag linkTag => GetLinkWrapper(linkTag.Link)(words),
                _ => words
            };
        }
    }
}