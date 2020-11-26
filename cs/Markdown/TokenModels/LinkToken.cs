using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class LinkToken : IToken
    {
        public static string MdTag => "[";

        public int MdTokenLength => "[".Length + Link.MdTokenLength + "]".Length +
                                    "(".Length + Description.MdTokenLength + ")".Length;

        private StringToken Link { get; }
        private StringToken Description { get; }

        private LinkToken(string description, string link)
        {
            Description = StringToken.Create(description);
            Link = StringToken.Create(link);
        }

        public string ToHtmlString() => $"<a href=\"{Link.ToHtmlString()}\">{Description.ToHtmlString()}</a>";

        public static LinkToken Create(string mdString, int startIndex)
        {
            var analyzer = new StringAnalyzer(mdString);
            var descriptionStart = startIndex + "[".Length;
            var descriptionEnd = mdString.IndexOf(']', descriptionStart);

            ThrowArgumentExceptionIfIncorrectDescription(analyzer, startIndex, descriptionEnd);

            var linkStart = descriptionEnd + 1 + "(".Length;
            var linkEnd = mdString.IndexOf(')', linkStart);

            ThrowArgumentExceptionIfIncorrectLink(analyzer, linkStart, linkEnd);

            var descriptionLength = descriptionEnd - descriptionStart;
            var description = mdString.Substring(descriptionStart, descriptionLength);

            var linkLength = linkEnd - linkStart;
            var link = mdString.Substring(linkStart, linkLength);
            return new LinkToken(description, link);
        }

        private static void ThrowArgumentExceptionIfIncorrectDescription(StringAnalyzer analyzer, int startIndex, int endIndex)
        {
            if (analyzer.AnalyzedString[startIndex] != '[')
                throw new ArgumentException($"{nameof(LinkToken)} should starts with open square bracket!");

            if (!analyzer.IsCharInsideValue(endIndex) || analyzer.AnalyzedString[endIndex] != ']')
                throw new ArgumentException($"{nameof(LinkToken)} should ends with end square bracket!");

            if (endIndex - startIndex <= 1)
                throw new ArgumentException($"Description in {nameof(LinkToken)} should has length more than 0!");

            if (analyzer.IsCharInsideValue(endIndex + 1) && analyzer.AnalyzedString[endIndex + 1] != '(')
                throw new ArgumentException(
                    $"{nameof(LinkToken)} should has opening parenthesis after closing square bracket!"
                );
        }

        private static void ThrowArgumentExceptionIfIncorrectLink(StringAnalyzer analyzer, int linkStart, int linkEnd)
        {
            if (!analyzer.IsCharInsideValue(linkEnd) || analyzer.AnalyzedString[linkEnd] != ')')
                throw new ArgumentException($"{nameof(LinkToken)} should ends with end parenthesis!");

            if (linkEnd - linkStart <= 1)
                throw new ArgumentException($"Link in {nameof(LinkToken)} should has length more than 0!");
        }
    }
}