using Markdown.MdTags.PairTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MdTagsConverters
{
    class MdTagsToHtmlTagsConverter
    {
        private static readonly Dictionary<Tag, Tag> mdTagToHtmlTag;

        static MdTagsToHtmlTagsConverter()
        {
            mdTagToHtmlTag = new Dictionary<Tag, Tag>();
            FillMdPairTags(mdTagToHtmlTag);
        }

        private static void FillMdPairTags(Dictionary<Tag, Tag> mdTagToHtmlTag)
        {
            var singleUnderline = new SingleUnderline();
            var doubleUnderline = new DoubleUnderline();

            mdTagToHtmlTag.Add(singleUnderline.Open, new Tag() { Id = "<em>", Value = "<em>" });
            mdTagToHtmlTag.Add(singleUnderline.Close, new Tag() { Id = "</em>", Value = "</em>" });

            mdTagToHtmlTag.Add(doubleUnderline.Open, new Tag() { Id = "<strong>", Value = "<strong>" });
            mdTagToHtmlTag.Add(doubleUnderline.Close, new Tag() { Id = "</strong>", Value = "</strong>" });
        }

        public static Tag GetHtmlTagByMdTag(Tag mdTag)
        {
            if (!mdTagToHtmlTag.ContainsKey(mdTag))
                throw new ArgumentException();
            return mdTagToHtmlTag[mdTag];
        }
    }
}