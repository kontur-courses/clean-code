using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagEm : TagConverter
    {
        public override TagHtml Html => TagHtml.em;
        public override TagMd Md => TagMd._;
        public override StringOfset Convert(string text, int position)
        {
            if (TextWithDigits(text, position))
                return new StringOfset(text[position].ToString(), 1);
            var result = new StringBuilder();
            int pos;
            for(pos = position + LengthMd; text.Substring(pos, LengthMd) != StringMd && pos < text.Length; pos++)
            {
                result.Append(text[pos].ToString());
            }
            var ofset = pos - position + LengthMd;
            if (pos == text.Length)
                return new StringOfset(GetResultWhetTetEnd(result), ofset);
            if (IsSpecialCpmbine())
                return new StringOfset(OpenTag() + result.ToString() + CloseTag(), ofset);
            if (ResultIsMoreThenOneWord(result))
                return new StringOfset(GetResultWithWhiteSpace(result, text, position, pos), ofset);
            return new StringOfset(FormTags(result), ofset);

            bool IsSpecialCpmbine() =>
                (position >= 2 && text.Substring(position - 2, 3) == "___") &&
                (pos <= text.Length - 3 && text.Substring(pos, 3) == "___");
        }
    }
}
