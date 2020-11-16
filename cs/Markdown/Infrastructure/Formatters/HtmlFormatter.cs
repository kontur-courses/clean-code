using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Formatters
{
    public class HtmlFormatter : IBlockFormatter
    {
        private readonly IWrapper wrapper;
        private readonly Dictionary<Style, Func<IEnumerable<string>, IEnumerable<string>>> generalWrappers;

        public HtmlFormatter(IWrapper wrapper)
        {
            this.wrapper = wrapper;
            generalWrappers = new Dictionary<Style, Func<IEnumerable<string>, IEnumerable<string>>>
            {
                {Style.None, wrapper.Wrap("", "")},
                {Style.Bold, wrapper.Wrap("<strong>", "</strong>")},
                {Style.Angled, wrapper.Wrap("<em>", "</em>")},
                {Style.Header, wrapper.Wrap("<h1>", "</h1>")}
            };
        }

        private Func<IEnumerable<string>, IEnumerable<string>> GetPictureWrapper(string description)
        {
            return wrapper.Wrap("<img src=\"", $"\" alt=\"{description}\">");
        }

        private Func<IEnumerable<string>, IEnumerable<string>> GetLinkWrapper(string link)
        {
            return wrapper.Wrap("<a href=\"", $"\">{link}</a>");
        }

        public IEnumerable<string> Format(Tag tag, IEnumerable<string> words)
        {
            if (generalWrappers.TryGetValue(tag.Style, out var wrap))
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