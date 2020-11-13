using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class Tag
    {
        public int StartOfOpeningTag;
        public int StartOfClosingTag;

        public abstract string MdTag { get; }
        public abstract string HtmlTagName { get; }

        public string OpeningHtmlTag => $"<{HtmlTagName}>";
        public string ClosingHtmlTag => $"</{HtmlTagName}>";

        public IEnumerable<int> OccupiedIndexes
        {
            get
            {
                return Enumerable.Range(StartOfOpeningTag, MdTag.Length)
                    .Concat(Enumerable.Range(StartOfClosingTag, MdTag.Length));
            }
        }

        public Tag(int startOfOpeningTag, int startOfClosingTag)
        {
            StartOfOpeningTag = startOfOpeningTag;
            StartOfClosingTag = startOfClosingTag;
        }

        public Tag()
        {
        }

        public bool IntersectWith(Tag tag)
        {
            return (tag.StartOfOpeningTag < StartOfOpeningTag && StartOfOpeningTag < tag.StartOfClosingTag &&
                    tag.StartOfClosingTag < StartOfClosingTag) ||
                   (StartOfOpeningTag < tag.StartOfOpeningTag && tag.StartOfOpeningTag < StartOfClosingTag &&
                    StartOfClosingTag < tag.StartOfClosingTag);
        }

        public bool Contains(Tag tag)
        {
            return StartOfOpeningTag < tag.StartOfOpeningTag && tag.StartOfClosingTag < StartOfClosingTag;
        }
    }
}