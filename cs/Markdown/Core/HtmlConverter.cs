using System;
using System.Collections.Generic;
using System.Text;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class HtmlConverter
    {
        private static Dictionary<string, Func<string, int, IToken>> TokensCreators { get; }
        private const char EscapeChar = '\\';

        static HtmlConverter()
        {
            TokensCreators = new Dictionary<string, Func<string, int, IToken>>
            {
                [HeaderToken.MdTag] = HeaderToken.Create,
                [ItalicToken.MdTag] = ItalicToken.Create,
                [BoldToken.MdTag] = BoldToken.Create,
                [LinkToken.MdTag] = LinkToken.Create
            };
        }

        public static string ConvertToHtmlString(string mdText)
        {
            if (mdText.StartsWith(HeaderToken.MdTag))
                return TokensCreators[HeaderToken.MdTag](mdText, 0).ToHtmlString();

            var collector = new StringBuilder();
            var pointer = 0;
            while (pointer < mdText.Length)
            {
                var stringUpToFirstTag = GetStringUpToNextTag(mdText, pointer, out var jumpToOpenTag);
                collector.Append(stringUpToFirstTag);
                pointer += jumpToOpenTag;

                if (pointer >= mdText.Length)
                    break;

                var token = GetNextToken(mdText, pointer);
                collector.Append(token.ToHtmlString());
                pointer += token.MdTokenLength;
            }

            return collector.ToString();
        }

        private static string GetStringUpToNextTag(string mdText, in int startIndex, out int jumpToOpenTag)
        {
            var analyzer = new StringAnalyzer(mdText);
            var collector = new StringBuilder();
            var currentIndex = startIndex;
            while (currentIndex < mdText.Length && !IsTokenStartTag(analyzer, currentIndex))
            {
                if (IsEscaped(analyzer, currentIndex))
                    currentIndex++;

                collector.Append(mdText[currentIndex]);
                currentIndex++;
            }

            jumpToOpenTag = currentIndex - startIndex;
            return collector.ToString();
        }

        private static IToken GetNextToken(string mdText, in int index)
        {
            try
            {
                var openTag = GetOpenTag(mdText, index);
                if (!TokensCreators.TryGetValue(openTag, out var tokenCreator))
                    throw new KeyNotFoundException($"Creator for {openTag} not found in {nameof(TokensCreators)}!");
                return tokenCreator(mdText, index);
            }
            catch (ArgumentException)
            {
                return StringToken.Create(mdText[index].ToString());
            }
        }

        private static string GetOpenTag(string mdText, int index)
        {
            var openTag = mdText[index].ToString();
            var singleTag = openTag;

            if (index + 1 >= 0 && index + 1 <= mdText.Length)
                openTag += mdText[index + 1];
            var possibleDoubleTag = openTag;

            return TokensCreators.ContainsKey(possibleDoubleTag) ? possibleDoubleTag : singleTag;
        }

        private static bool IsEscaped(StringAnalyzer analyzer, int i)
        {
            var canNextCharBeEscaped = analyzer.IsCharInsideValue(i + 1)
                                       && (TokensCreators.ContainsKey(analyzer.AnalyzedString[i + 1].ToString()) ||
                                           analyzer.AnalyzedString[i + 1] is EscapeChar);
            return analyzer.AnalyzedString[i] is EscapeChar && canNextCharBeEscaped;
        }

        private static bool IsTokenStartTag(StringAnalyzer analyzer, int i) =>
            TokensCreators.ContainsKey(analyzer.AnalyzedString[i].ToString())
            && IsNonWhiteSpaceCharAfterItalicTag(analyzer, i)
            && IsNonWhiteSpaceCharAfterBoldTag(analyzer, i);

        private static bool IsNonWhiteSpaceCharAfterBoldTag(StringAnalyzer analyzer, int i) =>
            analyzer.IsCharInsideValue(i + BoldToken.MdTag.Length) &&
            !analyzer.HasValueWhiteSpaceAt(i + BoldToken.MdTag.Length);

        private static bool IsNonWhiteSpaceCharAfterItalicTag(StringAnalyzer analyzer, int i) =>
            !analyzer.HasValueUnderscoreAt(i - 1) && analyzer.IsCharInsideValue(i + ItalicToken.MdTag.Length) &&
            !analyzer.HasValueWhiteSpaceAt(i + ItalicToken.MdTag.Length);
    }
}