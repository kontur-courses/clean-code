using System;
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

        private static HashSet<char> ServiceSymbols =>
            new HashSet<char> { EscapeCharacter, '_' }; // TODO: rewrite on reflection

        public static string RenderHtml(string markdownText) // TODO: decomposition?
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
            var allWraps = GetTextWraps(markdownText).Values.Aggregate(
                (wraps, otherWraps) =>
                    GetUnionOfWrapSequences(wraps, otherWraps)
                        .ToArray());


            throw new NotImplementedException();
        }

        private static Dictionary<ITextWrapType, TextWrap[]> GetTextWraps(string text)
        {
            var singleUnderscoreWrap = new SingleUnderscoreWrapType();
            var singleUnderscoreWrapFinder = new UnderscoresWrapFinder(singleUnderscoreWrap);
            var singleUnderscoreWraps = singleUnderscoreWrapFinder.GetPairsOfMarkers(text).ToArray();

            var doubleUnderscoresWrap = new DoubleUnderscoresWrapType();
            var doubleUnderscoresWrapFinder = new UnderscoresWrapFinder(doubleUnderscoresWrap,
                                                                        singleUnderscoreWraps);
            var doubleUnderscoresWraps = doubleUnderscoresWrapFinder.GetPairsOfMarkers(text).ToArray();

            return new Dictionary<ITextWrapType, TextWrap[]>
            {
                [singleUnderscoreWrap] = singleUnderscoreWraps,
                [doubleUnderscoresWrap] = doubleUnderscoresWraps
            };
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