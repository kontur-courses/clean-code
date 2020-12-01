using System;
using System.Text;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class HtmlConverter
    {
        private const char EscapeChar = '\\';

        public static string ConvertToHtmlString(string mdText)
        {
            if (mdText.StartsWith(HeaderToken.MdTag))
                return new HeaderToken(mdText, 0).ToHtmlString();

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
            while (currentIndex < mdText.Length && !TagsConfigReader.IsMarkdownTag(mdText[currentIndex].ToString()))
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
                var markdownTag = GetOpenTag(mdText, index);
                return TokenFactory.CreateNewToken(markdownTag, mdText, index);
            }
            catch (ArgumentException)
            {
                return new StringToken(mdText[index].ToString());
            }
        }

        private static string GetOpenTag(string mdText, in int index)
        {
            var openTag = mdText[index].ToString();
            var singleTag = openTag;

            if (index + 1 >= 0 && index + 1 < mdText.Length)
                openTag += mdText[index + 1];
            var possibleDoubleTag = openTag;

            return TagsConfigReader.IsMarkdownTag(possibleDoubleTag) ? possibleDoubleTag : singleTag;
        }

        private static bool IsEscaped(StringAnalyzer analyzer, in int index)
        {
            var mdString = analyzer.AnalyzedString;
            var canNextCharBeEscaped = analyzer.IsCharInsideValue(index + 1) &&
                                       (TagsConfigReader.IsMarkdownTag(mdString[index + 1].ToString()) ||
                                        mdString[index + 1] is EscapeChar);
            return mdString[index] is EscapeChar && canNextCharBeEscaped;
        }
    }
}