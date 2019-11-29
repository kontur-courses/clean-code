using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Wraps;

namespace MarkdownProcessor
{
    public class Tokenizer
    {
        private static readonly IWrapType textWrapType = new TextWrapType();
        private readonly IReadOnlyList<IWrapType> explicitWrapTypes;
        private readonly char escapeCharacter;

        public Tokenizer(IEnumerable<IWrapType> wrapTypes, char escapeCharacter)
        {
            this.escapeCharacter = escapeCharacter;

            // wrap type should have explicit borders, to determine it.
            explicitWrapTypes = wrapTypes.Where(wrapType => wrapType.OpenWrapMarker.Length *
                                                            wrapType.CloseWrapMarker.Length > 0)
                                         .ToArray();
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public List<Token> Process(string text)
        {
            var foundTokens = new List<Token>();
            var tokenCandidates = new Stack<Token>();
            var previousCharacterIsEscaping = false;

            var position = 0;
            while (position < text.Length)
            {
                if (CurrentCharacterIsEscaping())
                {
                    previousCharacterIsEscaping = true;
                    position++;
                    continue;
                }

                var wrapType = TryGetTheMostSpecificWrapType(position, text);

                if (WrapIsPotentialValid(wrapType))
                {
                    if (IsValidCloseMarker(position, text, tokenCandidates, wrapType))
                    {
                        CloseAndStoreFoundToken(position, text, tokenCandidates, foundTokens, wrapType,
                                                escapeCharacter);
                        position += wrapType.CloseWrapMarker.Length;
                        continue;
                    }

                    if (IsValidOpenMarker(position, text, wrapType))
                    {
                        TryToCloseOpenTextToken(position, text, tokenCandidates, foundTokens);
                        AddTokenCandidate(position, tokenCandidates, wrapType);
                        position += wrapType.OpenWrapMarker.Length;
                        continue;
                    }
                }

                if (tokenCandidates.Count == 0)
                    tokenCandidates.Push(new Token(position, textWrapType));
                position++;

                previousCharacterIsEscaping = false;
            }

            AddRestOfText(text, tokenCandidates, foundTokens, escapeCharacter);

            return foundTokens;

            bool CurrentCharacterIsEscaping() => !previousCharacterIsEscaping &&
                                                 text[position] == escapeCharacter;

            bool WrapIsPotentialValid(IWrapType wrapType) =>
                !previousCharacterIsEscaping &&
                !(wrapType is null) &&
                !WrapIsSurroundedByDigits(position, text, wrapType);
        }

        private IWrapType TryGetTheMostSpecificWrapType(int markerIndex, string text) =>
            explicitWrapTypes
                .Where(wrapType => TextContainsSubstring(markerIndex, text, wrapType.OpenWrapMarker))
                .OrderByDescending(wrapType => wrapType.OpenWrapMarker.Length)
                .FirstOrDefault();

        private static bool TextContainsSubstring(int startIndex, string text, string substring)
        {
            if (startIndex + substring.Length > text.Length)
                return false;

            return !substring.Where((character, index) => character != text[startIndex + index]).Any();
        }

        private static bool WrapIsSurroundedByDigits(int markerIndex, string text, IWrapType wrapType) =>
            WrapHasDigitBefore(markerIndex, text) &&
            WrapHasDigitAfter(markerIndex, text, wrapType);

        private static bool WrapHasDigitBefore(int markerIndex, string text) => markerIndex > 0 &&
                                                                                char.IsDigit(text[markerIndex - 1]);

        private static bool WrapHasDigitAfter(int markerIndex, string text, IWrapType mark) =>
            markerIndex + mark.OpenWrapMarker.Length < text.Length &&
            char.IsDigit(text[markerIndex + mark.OpenWrapMarker.Length]);

        private static void TryToCloseOpenTextToken(int markerIndex, string text,
                                                    Stack<Token> tokenCandidates, ICollection<Token> foundTokens)
        {
            if (!HasOpenTextToken()) return;

            var textToken = tokenCandidates.Pop();
            textToken.Content = text.Substring(textToken.ContentStartIndex, markerIndex - textToken.ContentStartIndex);

            foundTokens.Add(textToken);

            bool HasOpenTextToken() => tokenCandidates.Count > 0 &&
                                       tokenCandidates.Peek().WrapType.Equals(textWrapType);
        }

        private static bool IsValidCloseMarker(int markerIndex, string text,
                                               Stack<Token> tokenCandidates, IWrapType wrapType)
        {
            if (HasWhitespaceBefore(markerIndex, text) || tokenCandidates.Count == 0)
                return false;

            var openedToken = tokenCandidates.Peek();
            while (openedToken != null)
            {
                if (CloseMarkerHasAssociatedOpenMarker(openedToken))
                    return true;

                openedToken = openedToken.ParentToken;
            }

            return false;

            bool CloseMarkerHasAssociatedOpenMarker(Token associatedToken) =>
                associatedToken.WrapType.Equals(wrapType) &&
                associatedToken.ContentStartIndex != markerIndex;
        }

        private static void CloseAndStoreFoundToken(int markerIndex, string text, Stack<Token> tokenCandidates,
                                                    ICollection<Token> foundTokens, IWrapType wrapType,
                                                    char escapeCharacter)
        {
            var previousToken = tokenCandidates.Pop();
            while (!previousToken.WrapType.Equals(wrapType))
                previousToken = tokenCandidates.Pop();

            var contentLength = markerIndex - previousToken.ContentStartIndex;

            previousToken.Content = GetSubstringWithoutRedundantEscapeCharacters(
                previousToken.ContentStartIndex, contentLength,
                text, escapeCharacter);

            if (previousToken.ParentToken == null)
                foundTokens.Add(previousToken);
            else
                previousToken.ParentToken.ChildTokens.Add(previousToken);
        }

        private static bool IsValidOpenMarker(int markerIndex, string text, IWrapType wrapType) =>
            !HasWhitespaceAfter(markerIndex, text, wrapType.OpenWrapMarker.Length) ||
            markerIndex + wrapType.OpenWrapMarker.Length >= text.Length;

        private static void AddTokenCandidate(int markerIndex, Stack<Token> tokenCandidates, IWrapType wrapType)
        {
            var parentToken = tokenCandidates.Count == 0 ? null : tokenCandidates.Peek();
            var token = new Token(markerIndex, wrapType, parentToken);

            tokenCandidates.Push(token);
        }

        private static bool HasWhitespaceBefore(int markerIndex, string text) =>
            markerIndex - 1 >= 0 && char.IsWhiteSpace(text[markerIndex - 1]);

        private static bool HasWhitespaceAfter(int markerIndex, string text, int markerLength) =>
            markerIndex + markerLength < text.Length && char.IsWhiteSpace(text[markerIndex + markerLength]);

        private static void AddRestOfText(string text, Stack<Token> tokenCandidates,
                                          ICollection<Token> foundTokens, char escapeCharacter)
        {
            if (tokenCandidates.Count == 0) return;

            while (tokenCandidates.Count > 1)
                tokenCandidates.Pop();

            var lastToken = tokenCandidates.Pop();
            var token = new Token(lastToken.ContentStartIndex - lastToken.WrapType.OpenWrapMarker.Length, textWrapType);

            var contentLength = text.Length - token.ContentStartIndex;
            token.Content = GetSubstringWithoutRedundantEscapeCharacters(token.ContentStartIndex, contentLength,
                                                                         text, escapeCharacter);

            foundTokens.Add(token);
        }

        private static string GetSubstringWithoutRedundantEscapeCharacters(int startIndex, int substringLength,
                                                                           string text, char escapeCharacter,
                                                                           bool firstIsNotEscaping = true)
        {
            var defaultValue = firstIsNotEscaping ? text[startIndex].ToString() : string.Empty;
            var escapedStringBuilder = new StringBuilder(defaultValue, text.Length);

            var position = firstIsNotEscaping ? startIndex + 1 : startIndex;
            var previousCharacterIsEscaping = false;

            while (position < startIndex + substringLength && position < text.Length)
            {
                var currentCharacter = text[position];

                var currentCharacterIsEscaping = !previousCharacterIsEscaping &&
                                                 currentCharacter == escapeCharacter &&
                                                 position + 1 < text.Length;

                if (!currentCharacterIsEscaping)
                    escapedStringBuilder.Append(currentCharacter);

                position++;
                previousCharacterIsEscaping = currentCharacterIsEscaping;
            }

            return escapedStringBuilder.ToString();
        }
    }
}