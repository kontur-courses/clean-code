using System.Collections.Generic;
using System.Linq;
using Markdown.Data.TagsInfo;
using Markdown.Data.TagsInfo.Headings;
using Markdown.Data.TagsInfo.StandardTags;

namespace Markdown.Data
{
    public class MarkdownLanguage
    {
        private readonly Tag[] tags =
        {
            new Tag(new ItalicTagInfo(), "em"),
            new Tag(new BoldTagInfo(), "strong"),
            new Tag(new H1TagInfo(), "h1"),
            new Tag(new H2TagInfo(), "h2"),
            new Tag(new H3TagInfo(), "h3"),
            new Tag(new H4TagInfo(), "h4"),
            new Tag(new H5TagInfo(), "h5"),
            new Tag(new H6TagInfo(), "h6"),
        };

        public IEnumerable<string> GetAllTags =>
            tags.Select(tag => tag.Info.OpeningTag).Concat(tags.Select(tag => tag.Info.ClosingTag));

        public IEnumerable<TagTranslationInfo> GetTranslations => tags.Select(tag => tag.ToTranslationInfo);

        public IEnumerable<ITagInfo> GetTagsInfo => tags.Select(tag => tag.Info);
    }
}