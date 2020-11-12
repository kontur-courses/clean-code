using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagStrong : TagConverter
    {
        public override TagHtml Html => TagHtml.strong;

        public override TagMd Md => TagMd.__;

        private static readonly HashSet<string> tags = new HashSet<string>() { "_" };

        public override StringOfset Convert(string text, int position)
        {
            if (!TagCanOpen(text, position))
                return new StringOfset(StringMd, LengthMd);
            if (IsEmptyInsideTag(text, position))
                return GetTagEmptyInside();
            if (TextWithDigits(text, position))
                return new StringOfset(text[position].ToString(), 1);
            var result = new StringBuilder();
            int pos;
            int ofsetIndex;
            string tag;
            for (pos = position + LengthMd; CanItterate(); pos += ofsetIndex)
            {
                tag = TagsAssociation.GetTagMd(text, pos, tags);
                if (tag != null)
                {
                    var stringOfset = TagsAssociation.tagConverters[tag].Convert(text, pos);
                    result.Append(stringOfset.text);
                    ofsetIndex = stringOfset.ofset;
                    continue;
                }
                result.Append(text[pos].ToString());
                ofsetIndex = 1;
            }
            var ofset = pos - position + LengthMd;
            if (pos >= text.Length - LengthMd && !(pos <= text.Length - LengthMd && text.Substring(pos, LengthMd) == StringMd))
                return new StringOfset(GetResultWhetTextEnd(result), ofset - LengthMd);
            if (ResultIsMoreThenOneWord(result))
                return new StringOfset(GetResultWithWhiteSpace(result, text, position, pos), ofset);
            return new StringOfset(FormTags(result), ofset);

            bool CanItterate() =>
                pos < text.Length - LengthMd &&
                (text.Substring(pos, LengthMd) != StringMd || !TagCanClose(text, pos));
        }
    }
}
