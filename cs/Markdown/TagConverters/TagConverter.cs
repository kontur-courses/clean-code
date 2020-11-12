using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal abstract class TagConverter : ITagConverter
    {
        public abstract TagHtml Html { get; }
        public abstract TagMd Md { get; }
        public string StringMd => Md.ToString();
        public int LengthMd => StringMd.Length;

        public string StringHtml => Html.ToString();
        public string OpenTag() => string.Format("<{0}>", StringHtml);
        public string CloseTag() => string.Format(@"<\{0}>", StringHtml);

        public string FormTags(StringBuilder result) => OpenTag() + result.ToString() + CloseTag();
        public abstract StringOfset Convert(string text, int position);

        public bool PositionInCenterWord(string text, int position) =>
                (position > 1 && !char.IsWhiteSpace(text[position - 1])) &&
                (position < text.Length - LengthMd && !char.IsWhiteSpace(text[position + LengthMd]));

        public bool TextWithDigits(string text, int position) =>
            (position > 1 && char.IsDigit(text[position - 1])) ||
            (position < text.Length - LengthMd && char.IsDigit(text[position + LengthMd]));

        public string GetResultWhetTetEnd(StringBuilder result) => StringMd + result.ToString();

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

        public string GetResultWithWhiteSpace(StringBuilder result, string text, int posStart, int posEnd)
        {
            if (PositionInCenterWord(text, posStart) || PositionInCenterWord(text, posEnd))
                return StringMd + result.ToString() + StringMd;
            result.Append(CloseTag());
            return OpenTag() + result.ToString();
        }
    }
}
