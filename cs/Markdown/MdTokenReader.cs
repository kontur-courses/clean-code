using System;

namespace Markdown
{
    public class MdTokenReader : TokenReader
    {
        public MdTokenReader(string text) : base(text)
        {
            AddToken(ReadDigitToken);

            AddBasicToken<MdHeaderToken>("\n# ", "\n");
            AddBasicToken<MdBoldToken>("__", "__", typeof(MdItalicToken));
            AddBasicToken<MdItalicToken>("_", "_");
        }

        public static MdDigitToken ReadDigitToken(TokenReader reader, Token parent)
        {
            if (!reader.IsAfterSpace()) return null;
            var hasDigits = false;
            var position = reader.CurrentPosition;
            var offset = 0;

            for (; !reader.IsAtSpace(offset); offset++)
                hasDigits |= char.IsDigit(reader.Text[position + offset]);

            if (!hasDigits) return null;
            reader.CurrentPosition += offset;
            return new MdDigitToken {StartPosition = position, Length = offset};
        }
    }
}