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
            for(pos = position + LengthMd; text.Substring(pos, LengthMd) != StringMd; pos++)
            {
                result.Append(text[pos].ToString());
            }
            var ofset = pos - position + LengthMd;
            if (IsSpecialCpmbine())
                return new StringOfset(OpenTag() + result.ToString() + CloseTag(), ofset);
            if (ResultIsMoreThenOneWord())
                return new StringOfset(GetResultWithWhiteSpace(), ofset);
            result.Append(CloseTag());
            return new StringOfset(OpenTag() + result.ToString(), ofset);

            bool ResultIsMoreThenOneWord() 
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

            string GetResultWithWhiteSpace()
            {
                if (PositionInCenterWord(text, position) || PositionInCenterWord(text, pos))
                    return StringMd + result.ToString() + StringMd;
                result.Append(CloseTag());
                return OpenTag() + result.ToString();
            }

            bool IsSpecialCpmbine() =>
                (position >= 2 && text.Substring(position - 2, 3) == "___") &&
                (pos <= text.Length - 3 && text.Substring(pos, 3) == "___");
        }
    }
}
