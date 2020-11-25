using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        public static string MdTag => "_";
        string IToken.MdTag => MdTag;

        public int MdTokenLength { get; }
        private StringToken Children { get; }

        private ItalicToken(StringToken children, int rawTokenLength)
        {
            Children = children;
            MdTokenLength = MdTag.Length + rawTokenLength + MdTag.Length;
        }

        public string ToHtmlString() => $"<em>{Children.ToHtmlString()}</em>";

        public static ItalicToken Create(string mdString, int startIndex)
        {
            var endIndex = GetEndOfToken(mdString, startIndex);

            var rawToken = mdString
                .Substring(startIndex + 1, endIndex - startIndex - 1)
                .Replace("__", @"\_\_");
            var rawStringToken = HtmlConverter.ConvertToHtmlString(rawToken);
            return new ItalicToken(StringToken.Create(rawStringToken), rawStringToken.Length);
        }

        private static int GetEndOfToken(string mdString, int startIndex)
        {
            var analyzer = new StringAnalyzer(mdString);
            var endIndex = startIndex + 2;
            var hasIntersectionWithBoldTag = false;

            while (analyzer.IsCharInsideValue(endIndex) && !analyzer.HasValueUnderscoreAt(endIndex))
            {
                ++endIndex;
                if (analyzer.HasValueUnderscoreAt(endIndex) && analyzer.HasValueUnderscoreAt(endIndex + 1))
                {
                    endIndex += 2;
                    hasIntersectionWithBoldTag = !hasIntersectionWithBoldTag;
                }
            }

            TokenThrowHelper.AssertThatExtractedItalicTokenCorrect(analyzer, startIndex, endIndex,
                hasIntersectionWithBoldTag);
            return endIndex;
        }
    }
}