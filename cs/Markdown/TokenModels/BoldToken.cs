using Markdown.Core;

namespace Markdown.TokenModels
{
    public class BoldToken : IToken
    {
        private StringToken Children { get; }
        public int MdTokenLength { get; }
        public string ToHtmlString() => $"<strong>{Children.ToHtmlString()}</strong>";

        private BoldToken(StringToken children, int rawTokenLength)
        {
            Children = children;
            MdTokenLength = "__".Length + rawTokenLength + "__".Length;
        }

        public static BoldToken Create(string mdString, int startIndex)
        {
            var endIndex = GetTokenEndIndex(mdString, startIndex);
            var rawToken = mdString.Substring(startIndex + 2, endIndex - startIndex - 2);
            var rawStringToken = HtmlConverter.ConvertToHtmlString(rawToken);
            return new BoldToken(StringToken.Create(rawStringToken), rawToken.Length);
        }

        private static int GetTokenEndIndex(string mdString, int startIndex)
        {
            var endIndex = startIndex + 2;
            var hasIntersectionWithItalicTag = false;

            while (mdString.IsCharInsideString(endIndex + 1) && !AreDoubleUnderscore(mdString, endIndex))
            {
                endIndex++;
                if (mdString.HasUnderscoreAt(endIndex) && !mdString.HasUnderscoreAt(endIndex + 1))
                    hasIntersectionWithItalicTag = !hasIntersectionWithItalicTag;
            }

            TokenThrowHelper.AssertThatExtractedBoldTokenCorrect(mdString, startIndex, endIndex,
                hasIntersectionWithItalicTag);
            return endIndex;
        }

        private static bool AreDoubleUnderscore(string mdString, int endIndex) =>
            mdString.HasUnderscoreAt(endIndex) && mdString.HasUnderscoreAt(endIndex + 1);
    }
}