using System.Linq;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class BoldToken : IToken
    {
        public static string MdTag => "__";
        string IToken.MdTag => MdTag;

        public int MdTokenLength { get; }
        public string ToHtmlString() => $"<strong>{Children.ToHtmlString()}</strong>";
        private StringToken Children { get; }

        private BoldToken(StringToken children, int rawTokenLength)
        {
            Children = children;
            MdTokenLength = MdTag.Length + rawTokenLength + MdTag.Length;
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
            var analyzer = new StringAnalyzer(mdString);
            var endIndex = startIndex + 2;
            var hasIntersectionWithItalicTag = false;

            while (analyzer.IsCharInsideValue(endIndex + 1) && !AreDoubleUnderscore(analyzer, endIndex))
            {
                endIndex++;
                if (analyzer.HasValueUnderscoreAt(endIndex) && !analyzer.HasValueUnderscoreAt(endIndex + 1))
                    hasIntersectionWithItalicTag = !hasIntersectionWithItalicTag;
            }

            TokenThrowHelper.AssertThatExtractedBoldTokenCorrect(analyzer, startIndex, endIndex,
                hasIntersectionWithItalicTag);
            return endIndex;
        }

        private static bool AreDoubleUnderscore(StringAnalyzer analyzer, int endIndex) =>
            analyzer.HasValueUnderscoreAt(endIndex) && analyzer.HasValueUnderscoreAt(endIndex + 1);
    }
}