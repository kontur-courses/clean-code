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
            this.escapeCharacter = escapeCharacter;
            this.tokenWrapTypes = tokenWrapTypes;
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public List<Token> Process(string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<Token>();

            var foundTokens = new List<Token>();
            var tokenCandidates = new Stack<Token>();

            var reader = new Reader(0, text, false, escapeCharacter);

            tokenCandidates.Push(new Token(0, textWrapType)); // to store text before first token

            var markerPosition = reader.ReadUntil(characterContext => IsMarker(characterContext, tokenCandidates));

            while (markerPosition != -1)
            {
                var wrapType = TryGetTheMostSpecificWrapType(markerPosition, text);

                if (IsValidCloseMarker(markerPosition, text, tokenCandidates) &&
                    CloseMarkerHasAssociatedOpenMarker(markerPosition, text, tokenCandidates))
                {
                    var closedToken = CloseFoundToken(tokenCandidates, wrapType);
                    StoreClosedToken(closedToken, markerPosition, text, foundTokens, escapeCharacter);

                    if (tokenCandidates.Count == 0)
                        OpenTextToken(markerPosition, wrapType, tokenCandidates);

                    reader.Skip(wrapType.CloseWrapMarker.Length - 1);
                }
                else if (IsValidOpenMarker(markerPosition, text))
                {
                    TryToCloseOpenTextToken(markerPosition, text, tokenCandidates, foundTokens);
                    AddTokenCandidate(markerPosition, tokenCandidates, wrapType);

                    reader.Skip(wrapType.OpenWrapMarker.Length - 1);
                }
                else if (IsValidCloseMarker(markerPosition, text, tokenCandidates))
                    reader.Skip(wrapType.CloseWrapMarker.Length - 1);

                markerPosition = reader.ReadUntil(characterContext => IsMarker(characterContext, tokenCandidates));
            }

            AddRestOfText(text, tokenCandidates, foundTokens, escapeCharacter);

            return foundTokens;
        }

        private bool IsMarker(CharacterContext characterContext, IReadOnlyCollection<Token> tokenCandidates) =>
            IsValidCloseMarker(characterContext, tokenCandidates) || IsValidOpenMarker(characterContext);

        private bool IsValidOpenMarker(CharacterContext context) =>
            WrapIsPotentialValid(context.Position, context.Text, context.PreviousCharacterIsEscaping) &&
            IsValidOpenMarker(context.Position, context.Text);

        private bool IsValidCloseMarker(CharacterContext context, IReadOnlyCollection<Token> tokenCandidates) =>
            WrapIsPotentialValid(context.Position, context.Text, context.PreviousCharacterIsEscaping) &&
            IsValidCloseMarker(context.Position, context.Text, tokenCandidates);

        private bool WrapIsPotentialValid(int position, string text, bool previousCharacterIsEscaping)
        {
            var wrapType = TryGetTheMostSpecificWrapType(position, text);

            return !previousCharacterIsEscaping &&
                   !(wrapType is null) &&
                   !WrapIsSurroundedByDigits(position, text, wrapType);
        }

        private IWrapType TryGetTheMostSpecificWrapType(int markerIndex, string text) =>
            tokenWrapTypes
                .Where(wrapType => TextContainsSubstring(markerIndex, text, wrapType.OpenWrapMarker))
                .OrderByDescending(wrapType => wrapType.OpenWrapMarker.Length)
                .FirstOrDefault();

        private static void OpenTextToken(int markerIndex, IWrapType wrapType, Stack<Token> tokenCandidates)
        {
            var textTokenStart = markerIndex + wrapType.CloseWrapMarker.Length;
            tokenCandidates.Push(new Token(textTokenStart, textWrapType));
        }

        private static bool TextContainsSubstring(int startIndex, string text, string substring)
        {
            if (startIndex + substring.Length > text.Length)
                return false;

            return !substring.Where((character, index) => character != text[startIndex + index]).Any();
        }

        private static bool WrapIsSurroundedByDigits(int markerIndex, string text, IWrapType wrapType) =>
            WrapHasDigitBefore(markerIndex, text) &&
            WrapHasDigitAfter(markerIndex, text, wrapType);

        private static bool WrapHasDigitBefore(int markerIndex, string text) =>
            markerIndex > 0 && char.IsDigit(text[markerIndex - 1]);

        private static bool WrapHasDigitAfter(int markerIndex, string text, IWrapType mark) =>
            markerIndex + mark.OpenWrapMarker.Length < text.Length &&
            char.IsDigit(text[markerIndex + mark.OpenWrapMarker.Length]);

        private void TryToCloseOpenTextToken(
            int markerIndex, string text, Stack<Token> tokenCandidates, ICollection<Token> foundTokens)
        {
            if (!HasOpenTextToken(tokenCandidates)) return;

            var textToken = tokenCandidates.Pop();
            var contentLength = markerIndex - textToken.ContentStartIndex;

            textToken.Content = GetSubstringWithoutRedundantEscapeCharacters(
                textToken.ContentStartIndex, contentLength, text, escapeCharacter);

            if (!string.IsNullOrEmpty(textToken.Content))
                foundTokens.Add(textToken);
        }

        private static bool HasOpenTextToken(Stack<Token> tokenCandidates) =>
            tokenCandidates.Count > 0 && tokenCandidates.Peek().WrapType.Equals(textWrapType);

        private static bool IsValidCloseMarker(int markerIndex,
                                               string text,
                                               IReadOnlyCollection<Token> tokenCandidates) =>
            tokenCandidates.Count > 0 && !HasWhitespaceBefore(markerIndex, text);

        private bool CloseMarkerHasAssociatedOpenMarker(int markerIndex, string text, Stack<Token> tokenCandidates)
        {
            var wrapType = TryGetTheMostSpecificWrapType(markerIndex, text);
            var openedToken = tokenCandidates.Peek();
            while (openedToken != null)
            {
                if (IsCloseMarkerAssociatedWithOpenMarker(openedToken, wrapType, markerIndex))
                    return true;

                openedToken = openedToken.ParentToken;
            }

            return false;
        }

        private static bool IsCloseMarkerAssociatedWithOpenMarker(Token associatedToken,
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

        private bool IsValidOpenMarker(int markerIndex, string text)
        {
            var wrapType = TryGetTheMostSpecificWrapType(markerIndex, text);

            return !HasWhitespaceAfter(markerIndex, text, wrapType.OpenWrapMarker.Length) &&
                   markerIndex + wrapType.OpenWrapMarker.Length < text.Length - 1;
        }

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
            var textTokenStart = lastToken.ContentStartIndex - lastToken.WrapType.OpenWrapMarker.Length;

            var textToken = new Token(textTokenStart, textWrapType);

            var contentLength = text.Length - textToken.ContentStartIndex;

            textToken.Content = GetSubstringWithoutRedundantEscapeCharacters(
                textToken.ContentStartIndex, contentLength, text, escapeCharacter);

            foundTokens.Add(textToken);
        }

        private static string GetSubstringWithoutRedundantEscapeCharacters(
            int startIndex, int substringLength, string text, char escapeCharacter)
        {
            var escapedStringBuilder = new StringBuilder(text.Length);

            var position = startIndex;
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