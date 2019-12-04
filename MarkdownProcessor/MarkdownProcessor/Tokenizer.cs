using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownProcessor.Wraps;

namespace MarkdownProcessor
{
    public class Tokenizer
    {
        private static readonly IWrapType textWrapType = new TextWrapType();
        private readonly IReadOnlyList<IWrapType> tokenWrapTypes;
        private readonly char escapeCharacter;

        public Tokenizer(IReadOnlyList<IWrapType> tokenWrapTypes, char escapeCharacter)
        {
            if (tokenWrapTypes.Any(wrapType => string.IsNullOrEmpty(wrapType.OpenWrapMarker) ||
                                               string.IsNullOrEmpty(wrapType.CloseWrapMarker)))
                throw new ArgumentException("Wrap type should have explicit markers, to determine it," +
                                            "but was passed null or empty wrap marker.", nameof(tokenWrapTypes));

            this.escapeCharacter = escapeCharacter;
            this.tokenWrapTypes = tokenWrapTypes;
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
                if (CurrentCharacterIsEscaping(previousCharacterIsEscaping, position, text))
                {
                    previousCharacterIsEscaping = true;
                    position++;
                    continue;
                }

                var wrapType = TryGetTheMostSpecificWrapType(position, text);

                if (WrapIsPotentialValid(wrapType, previousCharacterIsEscaping, position, text))
                {
                    if (IsValidCloseMarker(position, text, tokenCandidates, wrapType))
                    {
                        var closedToken = CloseFoundToken(tokenCandidates, wrapType);
                        StoreClosedToken(closedToken, position, text, foundTokens, escapeCharacter);

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
        }

        private static bool WrapIsPotentialValid(IWrapType wrapType,
                                                 bool previousCharacterIsEscaping,
                                                 int position,
                                                 string text) =>
            !previousCharacterIsEscaping &&
            !(wrapType is null) &&
            !WrapIsSurroundedByDigits(position, text, wrapType);

        private bool CurrentCharacterIsEscaping(bool previousCharacterIsEscaping, int position, string text) =>
            !previousCharacterIsEscaping && text[position] == escapeCharacter;

        private IWrapType TryGetTheMostSpecificWrapType(int markerIndex, string text) =>
            tokenWrapTypes
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

        private static void TryToCloseOpenTextToken(
            int markerIndex, string text, Stack<Token> tokenCandidates, ICollection<Token> foundTokens)
        {
            if (!HasOpenTextToken(tokenCandidates)) return;

            var textToken = tokenCandidates.Pop();
            textToken.Content = text.Substring(textToken.ContentStartIndex, markerIndex - textToken.ContentStartIndex);

            foundTokens.Add(textToken);
        }

        private static bool HasOpenTextToken(Stack<Token> tokenCandidates) =>
            tokenCandidates.Count > 0 && tokenCandidates.Peek().WrapType.Equals(textWrapType);

        private static bool IsValidCloseMarker(
            int markerIndex, string text, Stack<Token> tokenCandidates, IWrapType wrapType)
        {
            if (HasWhitespaceBefore(markerIndex, text) || tokenCandidates.Count == 0)
                return false;

            var openedToken = tokenCandidates.Peek();
            while (openedToken != null)
            {
                if (CloseMarkerHasAssociatedOpenMarker(openedToken, wrapType, markerIndex))
                    return true;

                openedToken = openedToken.ParentToken;
            }

            return false;
        }

        private static bool CloseMarkerHasAssociatedOpenMarker(Token associatedToken,
                                                               IWrapType wrapType,
                                                               int markerIndex) =>
            associatedToken.WrapType.Equals(wrapType) &&
            associatedToken.ContentStartIndex != markerIndex;

        private static Token CloseFoundToken(Stack<Token> tokenCandidates, IWrapType wrapType)
        {
            var previousToken = tokenCandidates.Pop();
            while (!previousToken.WrapType.Equals(wrapType))
                previousToken = tokenCandidates.Pop();

            return previousToken;
        }

        private static void StoreClosedToken(
            Token closedToken, int markerIndex, string text, ICollection<Token> foundTokens, char escapeCharacter)
        {
            var contentLength = markerIndex - closedToken.ContentStartIndex;

            closedToken.Content = GetSubstringWithoutRedundantEscapeCharacters(
                closedToken.ContentStartIndex, contentLength,
                text, escapeCharacter);

            if (closedToken.ParentToken == null)
                foundTokens.Add(closedToken);
            else
                closedToken.ParentToken.ChildTokens.Add(closedToken);
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

        private static void AddRestOfText(
            string text, Stack<Token> tokenCandidates, ICollection<Token> foundTokens, char escapeCharacter)
        {
            if (tokenCandidates.Count == 0) return;

            while (tokenCandidates.Count > 1)
                tokenCandidates.Pop();

            var lastToken = tokenCandidates.Pop();
            var token = new Token(lastToken.ContentStartIndex - lastToken.WrapType.OpenWrapMarker.Length, textWrapType);

            var contentLength = text.Length - token.ContentStartIndex;

            token.Content = GetSubstringWithoutRedundantEscapeCharacters(
                token.ContentStartIndex, contentLength, text, escapeCharacter);

            foundTokens.Add(token);
        }

        private static string GetSubstringWithoutRedundantEscapeCharacters(
            int startIndex, int substringLength, string text, char escapeCharacter, bool firstIsNotEscaping = true)
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