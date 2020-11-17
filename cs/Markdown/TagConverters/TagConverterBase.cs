using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal abstract class TagConverterBase : ITagConverter
    {
        public abstract bool IsSingleTag { get; }
        public abstract HashSet<string> TagInside { get; }

        public abstract bool IsTag(string text, int pos);

        public abstract bool CanOpen(StringBuilder text, int pos);
        public abstract bool CanClose(StringBuilder text, int pos);

        internal bool CanProcessTag(string tag) => TagInside.Contains(tag);

        public abstract string Html { get; }
        public abstract string Md { get; }
        public string StringMd => Md.ToString();
        public int LengthMd => StringMd.Length;

        public string StringHtml => Html.ToString();

        public string OpenTag() => string.Format("<{0}>", StringHtml);
        public string CloseTag() => string.Format(@"<\{0}>", StringHtml);

        public StringBuilder FormTags(StringBuilder text)
        {
            var result = new StringBuilder(OpenTag());
            for(var i = LengthMd; i < text.Length - LengthMd; i++)
                result.Append(text[i]);
            result.Append(CloseTag());
            return result;
        }
        public StringBuilder Convert(StringBuilder tagsText, StringBuilder text, int start, int finish)
        {
            if (IsSingleTag)
            {
                var t = new StringBuilder();
                t.Append(tagsText);
                t.Append(StringMd);
                return FormTags(t);
            }
            if(StringMd == new TagUl().StringMd)
            {
                return (this as TagUl).Convert(tagsText);
            }
            if (ResultIsMoreThenOneWord(tagsText))
                return GetResultWithWhiteSpace(tagsText, text, start, finish);
            return FormTags(tagsText);
        }

        public bool PositionInCenterWord(StringBuilder text, int position) =>
                (position > 1 && !char.IsWhiteSpace(text[position - 1])) &&
                (position < text.Length - LengthMd && !char.IsWhiteSpace(text[position + LengthMd]));

        public bool TextWithDigits(string text, int position) =>
            (position > 1 && char.IsDigit(text[position - 1])) ||
            (position < text.Length - LengthMd && char.IsDigit(text[position + LengthMd]));

        public bool IsTagBase(string text, int position) => !TextWithDigits(text, position);

        public bool ResultIsMoreThenOneWord(StringBuilder result)
        {
            int i;
            for (i = 0; i < result.Length; i++)
                if (!char.IsWhiteSpace(result[i]))
                    break;
            for (; i < result.Length; i++)
                if (char.IsWhiteSpace(result[i]))
                    break;
            for (; i < result.Length; i++)
                if (!char.IsWhiteSpace(result[i]))
                    break;
            return i != result.Length;
        }

        public StringBuilder GetResultWithWhiteSpace(StringBuilder tagsText, StringBuilder text, int posStart, int posEnd)
        {
            if (PositionInCenterWord(text, posStart) || PositionInCenterWord(text, posEnd))
            {
                return tagsText;
            }
            return FormTags(tagsText);
        }

        public bool CanOpenBase(StringBuilder text, int pos) => 
            pos < text.Length - LengthMd && !char.IsWhiteSpace(text[pos + LengthMd]);
        public bool CanCloseBase(StringBuilder text, int pos) => !IsSingleTag &&  pos > 1 && !char.IsWhiteSpace(text[pos - 1]);
    }
}
