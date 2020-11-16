#nullable enable
namespace Markdown
{
    public class ImageTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            if (!parser.IsImageStaringAltTextTag(text, index))
                return null;

            var correctFormat = false;
            var value = "";
            for (var i = index + 2; i < text.Length; ++i)
            {
                if (parser.IsImageEndingAltTextTag(text, i))
                {
                    correctFormat = true;
                }

                if (!parser.IsImageEndingUrlTag(text, i) && !correctFormat)
                    continue;

                value = text.Substring(index, i - index + 1);
            }

            var token = new Token(index, value, TokenType.Image);
            return value != "" ? token : null;
        }
    }
}