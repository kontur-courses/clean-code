using System;
using System.Linq;

namespace Markdown
{
    public class MdTokenReader : TokenReader
    {
        public MdTokenReader(string text) : base(text)
        {
            TokenTypes.Add(new CustomTokenType(ReadEscapeChar));
            TokenTypes.Add(new CustomTokenType(ReadDigitToken));

            TokenTypes.Add(new BasicTokenType<MdHeaderToken>("\n# ", "\n"));

            var italic = new BasicTokenType<MdItalicToken>("_", "_");
            var bold = new BasicTokenType<MdBoldToken>("__", "__") {DisallowedTokenTypes = {italic}};

            TokenTypes.Add(bold);
            TokenTypes.Add(italic);

            TokenTypes.Add(new CustomTokenType(ReadLink));
        }

        public static EscapedStringToken ReadEscapeChar(TokenReader reader)
        {
            var token = new EscapedStringToken(reader.CurrentPosition, 2);
            if (!reader.TryGet("\\")) return null;
            reader.CurrentPosition++;
            if (!reader.TryGet("\\") && !reader.TokenTypes
                .Any(t => t is BasicTokenType b &&
                          (reader.TryGet(b.Start)
                           || reader.TryGet(b.End, b.EndWithNewLine)))) return null;
            reader.CurrentPosition++;
            return token;
        }

        public static MdDigitToken ReadDigitToken(TokenReader reader)
        {
            if (reader.IsAtSpace()) return null;
            var token = new MdDigitToken(reader.CurrentPosition);

            var hasDigits = false;
            while (!reader.IsAtSpace())
            {
                hasDigits |= char.IsDigit(reader.Text[reader.CurrentPosition]);
                reader.CurrentPosition++;
                token.Length++;
            }

            return hasDigits ? token : null;
        }

        public static MdLinkToken ReadLink(TokenReader reader)
        {
            var token = new MdLinkToken(reader.CurrentPosition);
            var linkTextType = new BasicTokenType<MdLinkToken.MdLinkTextToken>("[", "]");
            var linkUrlType = new BasicTokenType<MdLinkToken.MdLinkUrlToken>("(", ")");
            token.TextToken = reader.ReadTokenType(linkTextType);
            token.UrlToken = reader.ReadTokenType(linkUrlType);
            if (token.TextToken == null || token.UrlToken == null) return null;
            token.Length = token.TextToken.Length + token.UrlToken.Length;
            return token;
        }
    }
}