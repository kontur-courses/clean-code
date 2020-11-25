using System;
using System.Linq;
using System.Text;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class HtmlConverter
    {
        private static readonly char[] OpenTagChars = {'_', '['};
        private const char EscapeChar = '\\';

        public static string ConvertToHtmlString(string mdText)
        {
            if (string.IsNullOrEmpty(mdText))
                return string.Empty;

            if (mdText.StartsWith("# "))
                return HeaderToken.Create(mdText).ToHtmlString();

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
            var collector = new StringBuilder();
            var currentIndex = startIndex;
            while (currentIndex < mdText.Length && !IsTokenStartTag(mdText, currentIndex))
            {
                if (IsEscaped(mdText, currentIndex))
                    currentIndex++;

                collector.Append(mdText[currentIndex]);
                currentIndex++;
            }

            jumpToOpenTag = currentIndex - startIndex;
            return collector.ToString();
        }

        private static IToken GetNextToken(string mdText, int i)
        {
            try
            {
                return CreateToken(mdText, i);
            }
            catch (ArgumentException)
            {
                return StringToken.Create(mdText[i].ToString());
            }
        }

        private static IToken CreateToken(string mdText, in int index)
        {
            return mdText[index] switch
            {
                '_' when mdText.HasUnderscoreAt(index + 1) => BoldToken.Create(mdText, index),
                '_' => ItalicToken.Create(mdText, index),
                '[' => LinkToken.Create(mdText, index),
                _ => throw new ArgumentException($"{mdText[index]} wasn't open tag char!")
            };
        }

        private static bool IsEscaped(string mdText, int i)
        {
            var canNextCharBeEscaped = mdText.IsCharInsideString(i + 1)
                                       && (OpenTagChars.Contains(mdText[i + 1]) || mdText[i + 1] is EscapeChar);
            return mdText[i] is EscapeChar && canNextCharBeEscaped;
        }

        private static bool IsTokenStartTag(string mdText, int i) =>
            OpenTagChars.Contains(mdText[i])
            && !mdText.HasUnderscoreAt(i - 1) && mdText.HasNonWhiteSpaceAt(i + 1)
            && mdText.HasNonWhiteSpaceAt(i + 2);
    }
}