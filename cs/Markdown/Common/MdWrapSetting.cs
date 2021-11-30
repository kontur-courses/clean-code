using System;
using System.Collections.Generic;

namespace Markdown.Common
{
    public class MdWrapSetting
    {
        private readonly List<Func<string, Token, bool>> ignoreTagRules = new List<Func<string, Token, bool>>();

        public string MdTag { get; }
        private string HtmlOpenTag { get; }
        private string HtmlCloseTag { get; }
        public MdTagType TagType { get; }
        public IEnumerable<Func<string, Token, bool>> IgnoreTagRules => ignoreTagRules.AsReadOnly();


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

        public MdWrapSetting(string mdTag, string htmlOpenTag, string htmlCloseTag, MdTagType tagType,
            IEnumerable<Func<string, Token, bool>> ignoreTagRules)
            : this(mdTag, htmlOpenTag, htmlCloseTag, tagType)
        {
            this.ignoreTagRules.AddRange(ignoreTagRules);
        }


        public string RemoveMdTags(string value)
        {
            switch (TagType)
            {
                case MdTagType.Block:
                case MdTagType.Backslash:
                    return value.Remove(0, MdTag.Length);
                case MdTagType.Span:
                    return value.Remove(value.Length - MdTag.Length).Remove(0, MdTag.Length);
                default:
                    throw new NotImplementedException(
                        $"Невозможно выполнить {nameof(RemoveMdTags)}: операция для типа {TagType} не определена");
            }
        }

        public string InsertHtmlTags(string text)
        {
            switch (TagType)
            {
                case MdTagType.Block:
                    return text.Insert(text.EndsWith(Environment.NewLine)
                                ? text.Length - Environment.NewLine.Length
                                : text.Length,
                            HtmlCloseTag)
                        .Insert(0, HtmlOpenTag);
                case MdTagType.Backslash:
                case MdTagType.Span:
                    return text.Insert(text.Length, HtmlCloseTag).Insert(0, HtmlOpenTag);
                default:
                    throw new NotImplementedException(
                        $"Невозможно выполнить {nameof(InsertHtmlTags)}: операция для типа {TagType} не определена");
            }
        }
    }
}