using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownProcessor.TextWraps;
using MarkdownProcessor.WrapFinders;

namespace MarkdownProcessor
{
    public static class Markdown
    {
        public const char EscapeCharacter = '\\';

        public static HashSet<char> WhiteSpaceSymbols => new HashSet<char> { ' ', '\t', '\n' };

        private static readonly SingleUnderscoreWrapType singleUnderscoreWrapType = new SingleUnderscoreWrapType();
        private static readonly DoubleUnderscoresWrapType doubleUnderscoresWrapType = new DoubleUnderscoresWrapType();

        public static IReadOnlyDictionary<ITextWrapType, WrapFinder> WrapFinderByWrapType =>
            new Dictionary<ITextWrapType, WrapFinder>
            {
                [singleUnderscoreWrapType] = new UnderscoresWrapFinder(singleUnderscoreWrapType),
                [doubleUnderscoresWrapType] = new UnderscoresWrapFinder(doubleUnderscoresWrapType)
            };

        public static string RenderHtml(string markdownText)
        {
            var markdownWithHtmlTags = ReplaceMarkdownWrapsWithHtmlTags(markdownText);

            var htmlStringBuilder = new StringBuilder(markdownWithHtmlTags.Length);

            var position = 0;
            var previousCharacterIsEscaping = false;

            while (position < markdownWithHtmlTags.Length)
            {
                var currentCharacter = markdownWithHtmlTags[position];

                var currentCharacterIsEscaping = !previousCharacterIsEscaping && currentCharacter == EscapeCharacter;

                if (!currentCharacterIsEscaping)
                    htmlStringBuilder.Append(currentCharacter);

                position++;
                previousCharacterIsEscaping = currentCharacterIsEscaping;
            }

            return htmlStringBuilder.ToString();
        }

        private static string ReplaceMarkdownWrapsWithHtmlTags(string markdownText)
        {
            var allWraps = GetAllTextWraps(markdownText);
            var currentWrapIndex = -1;

            var stringBuilder = new StringBuilder(markdownText.Length);
            var position = 0;

            while (position < markdownText.Length)
            {
                if (CurrentCharacterIsNextOpenMarker())
                {
                    currentWrapIndex++;
                    stringBuilder.Append(allWraps[currentWrapIndex].WrapType.HtmlRepresentationOfOpenMarker);

                    position += allWraps[currentWrapIndex].WrapType.OpenWrapMarker.Length;

                    continue;
                }

                if (CurrentCharacterIsCloseMarker())
                {
                    stringBuilder.Append(allWraps[currentWrapIndex].WrapType.HtmlRepresentationOfCloseMarker);

                    position += allWraps[currentWrapIndex].WrapType.CloseWrapMarker.Length;
                    currentWrapIndex--;

                    continue;
                }

                stringBuilder.Append(markdownText[position]);

                position++;
            }

            return stringBuilder.ToString();

            bool CurrentCharacterIsNextOpenMarker() => currentWrapIndex + 1 < allWraps.Length &&
                                                       position == allWraps[currentWrapIndex + 1].OpenMarkerIndex;

            bool CurrentCharacterIsCloseMarker() => currentWrapIndex >= 0 &&
                                                    position == allWraps[currentWrapIndex].CloseMarkerIndex;
        }

        private static TextWrap[] GetAllTextWraps(string text)
        {
            var singleUnderscoreWraps = WrapFinderByWrapType[singleUnderscoreWrapType].GetWraps(text).ToArray();
            var representAsNullIfEmpty = singleUnderscoreWraps.Length == 0 ? null : singleUnderscoreWraps;
            var doubleUnderscoresWraps = WrapFinderByWrapType[doubleUnderscoresWrapType]
                                         .GetWraps(text, representAsNullIfEmpty).ToArray();

            return new[] { singleUnderscoreWraps, doubleUnderscoresWraps }
                .Aggregate((wraps, otherWraps) => GetUnionOfWrapSequences(wraps, otherWraps).ToArray());
        }

        private static IEnumerable<TextWrap> GetUnionOfWrapSequences(IReadOnlyList<TextWrap> wraps,
                                                                     IReadOnlyList<TextWrap> otherWraps)
        {
            var wrapsIndex = 0;
            var otherWrapsIndex = 0;

            while (wrapsIndex < wraps.Count && otherWrapsIndex < otherWraps.Count)
                if (wraps[wrapsIndex].OpenMarkerIndex < otherWraps[otherWrapsIndex].OpenMarkerIndex)
                    yield return wraps[wrapsIndex++];
                else
                    yield return otherWraps[otherWrapsIndex++];

            while (wrapsIndex < wraps.Count)
                yield return wraps[wrapsIndex++];
            while (otherWrapsIndex < otherWraps.Count)
                yield return otherWraps[otherWrapsIndex++];
        }
    }
}