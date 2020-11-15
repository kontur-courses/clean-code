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
                {Style.Header, Wrap("<h1>", "</h1>")},
            };
        }

        private Func<IEnumerable<string>, IEnumerable<string>> GetPictureWrapper(string description) 
            => Wrap("<img src=\"", $"\" alt=\"{description}\">");

        public override IEnumerable<string> Format(Tag tag, IEnumerable<string> words)
        {
            if (GeneralWrappers.TryGetValue(tag.Style, out var wrap))
                return wrap(words);

            if (tag is PictureTag pictureTag)
                return GetPictureWrapper(pictureTag.Description)(words);

            return words;
        }
    }
}