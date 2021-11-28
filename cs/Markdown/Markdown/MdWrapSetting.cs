using System;

namespace Markdown
{
    public class MdWrapSetting
    {
        public string MdTag { get; }
        public string MdCloseTag
        {
            get
            {
                switch (TagType)
                {
                    case MdTagType.Root:
                    case MdTagType.Block:
                        return Environment.NewLine;
                    case MdTagType.Span:
                        return MdTag;
                    default:
                        throw new NotImplementedException($"Невозможно определить {nameof(MdCloseTag)} для {TagType}.");
                };
            }
        }
        public string HtmlOpenTag { get; }
        public string HtmlCloseTag { get; }
        public MdTagType TagType { get; }

        public MdWrapSetting(string mdTag, MdTagType tagType)
        {
            MdTag = mdTag;
            TagType = tagType;
            HtmlOpenTag = string.Empty;
            HtmlCloseTag = string.Empty;
        }

        public MdWrapSetting(string mdTag, string htmlOpenTag, string htmlCloseTag, MdTagType tagType)
            : this(mdTag, tagType)
        {
            HtmlOpenTag = htmlOpenTag;
            HtmlCloseTag = htmlCloseTag;
        }
    }
}