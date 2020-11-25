using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        private StringToken Children { get; }
        public int MdTokenLength { get; }

        private ItalicToken(StringToken children, int rawTokenLength)
        {
            Children = children;
            MdTokenLength = "_".Length + rawTokenLength + "_".Length;
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
            var endIndex = startIndex + 2;
            var hasIntersectionWithBoldTag = false;

            while (mdString.IsCharInsideString(endIndex) && !mdString.HasUnderscoreAt(endIndex))
            {
                ++endIndex;
                if (mdString.HasUnderscoreAt(endIndex) && mdString.HasUnderscoreAt(endIndex + 1))
                {
                    endIndex += 2;
                    hasIntersectionWithBoldTag = !hasIntersectionWithBoldTag;
                }
            }

            TokenThrowHelper.AssertThatExtractedItalicTokenCorrect(mdString, startIndex, endIndex,
                hasIntersectionWithBoldTag);
            return endIndex;
        }
    }
}