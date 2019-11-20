using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tokenizer
    {
        private readonly IEnumerable<MarkTranslatorElement> _dictionary;
        private char _escapeСharacter;
        private int escapeСharacterCounter;

        public Tokenizer(IEnumerable<MarkTranslatorElement> dictionary, char escapeСharacter)
        {
            _dictionary = dictionary;
            _escapeСharacter = escapeСharacter;
        }

        public IEnumerable<Token> Parse(string text)
        {
            var result = new List<Token>();
            var openedTokens = new Stack<Token>();
            escapeСharacterCounter = 0;
            var index = 0;
            while (index < text.Length)
            {
                var isEscaped = EscapeCharacterCountIsOddBeforeDigit(text, index);
                var mark = isEscaped ? null : DefineTagAt(text, index);
                if (mark != null)
                {
                    ClosePreviousDefaultToken(openedTokens, result, index, text);
                    if (IsClosing(openedTokens, mark, text, index))
                        PlaceToken(openedTokens, result, mark, index, text);
                    else
                        PushTokenOnOpeningMark(openedTokens, mark, text, index);
                    index += mark.From.Opening.Length;
                }
                else
                {
                    if (openedTokens.Count == 0)
                        openedTokens.Push(new Token(index, MarkTranslatorElement.EmptyMark));
                    index++;
                }
            }

            CollapseRemainingTokensAndAdd(result, openedTokens, index, text);
            return TranseformUnableToParentingTokens(result);
        }

        #region TagStartChecking

        private bool EscapeCharacterCountIsOddBeforeDigit(string text, int index)
        {
            var isOdd = escapeСharacterCounter % 2 == 1;
            if (text[index] == _escapeСharacter)
                escapeСharacterCounter++;
            else escapeСharacterCounter = 0;
            return isOdd;
        }

        private MarkTranslatorElement DefineTagAt(string text, int index)
        {
            var tag = _dictionary
                .Where(item => IsTagInText(item.From.Opening, text, index))
                .OrderByDescending(item => item.From.Opening.Length)
                .FirstOrDefault();
            if (tag == null || IsBetweenDigits(tag, text, index))
                return null;
            return tag;
        }

        private static bool IsTagInText(string mark, string text, int index)
        {
            if (text.Length < index + mark.Length)
                return false;
            return !mark.Where((t, i) => text[index + i] != t).Any();
        }

        private static bool IsBetweenDigits(MarkTranslatorElement mark, string text, int index)
            => IsDigitBefore(text, index) && IsDigitAfter(mark, text, index);

        private static bool IsDigitBefore(string text, int index)
        {
            return index > 0
                   && char.IsDigit(text[index - 1]);
        }

        private static bool IsDigitAfter(MarkTranslatorElement mark, string text, int index)
        {
            return index + mark.From.Opening.Length < text.Length
                   && char.IsDigit(text[index + mark.From.Opening.Length]);
        }

        #endregion

        #region TokenReshator

        private static void ClosePreviousDefaultToken(Stack<Token> stack, ICollection<Token> tokens, int index,
            string text)
        {
            if (stack.Count == 0 || !stack.Peek().MarkToTranslate.Equals(MarkTranslatorElement.EmptyMark)) return;
            var token = stack.Pop();
            token.EndIndex = index - 1;
            token.Value = text.Substring(token.StartIndex, token.EndIndex - token.StartIndex + 1);
            tokens.Add(token);
        }

        private bool IsClosing(Stack<Token> openedTokens, MarkTranslatorElement mark, string text, int index)
        {
            var indexBeforeMark = index - 1;
            var isWhiteSpaceBefore = indexBeforeMark > 0 && char.IsWhiteSpace(text[indexBeforeMark]);
            if (isWhiteSpaceBefore || openedTokens.Count == 0)
                return false;
            var previousToken = openedTokens.Peek();
            while (previousToken != null)
            {
                if (previousToken.MarkToTranslate.From.Equals(mark.From) && previousToken.StartIndex != index)
                    return true;
                previousToken = previousToken.ParentToken;
            }

            return false;
        }

        private static void PlaceToken(Stack<Token> openedTokens, ICollection<Token> tokens, MarkTranslatorElement mark,
            int index, string text)
        {
            var previousToken = openedTokens.Pop();
            while (!previousToken.MarkToTranslate.From.Equals(mark.From))
                previousToken = openedTokens.Pop();
            previousToken.EndIndex = index - 1;
            previousToken.Value = text
                .Substring(previousToken.StartIndex, previousToken.EndIndex - previousToken.StartIndex + 1);
            if (previousToken.ParentToken == null)
                tokens.Add(previousToken);
            else
                previousToken.ParentToken.ChildTokens.Add(previousToken);
        }

        private void PushTokenOnOpeningMark(Stack<Token> openedTokens, MarkTranslatorElement mark, string text,
            int index)
        {
            var fatherToken = openedTokens.Count == 0 ? null : openedTokens.Peek();
            var token = new Token(index + mark.From.Opening.Length, mark, fatherToken);
            var indexAfterMark = token.StartIndex + token.MarkToTranslate.From.Opening.Length;
            if (indexAfterMark >= text.Length ||
                indexAfterMark < text.Length && !char.IsWhiteSpace(text[indexAfterMark]))
                openedTokens.Push(token);
        }

        #endregion

        private static void CollapseRemainingTokensAndAdd(ICollection<Token> tokens, Stack<Token> openedTokens,
            int index, string text)
        {
            if (openedTokens.Count == 0)
                return;
            while (openedTokens.Count != 1)
            {
                openedTokens.Pop();
            }

            var lastToken = openedTokens.Pop();
            var token = new Token(lastToken.StartIndex - lastToken.MarkToTranslate.From.Opening.Length,
                MarkTranslatorElement.EmptyMark) {EndIndex = index - 1};
            token.Value = text.Substring(token.StartIndex, token.EndIndex - token.StartIndex + 1);
            tokens.Add(token);
        }

        private IEnumerable<Token> TranseformUnableToParentingTokens(IEnumerable<Token> tokens)
        {
            return tokens.Select(t => TranseformUnableToParentingToken(t));
        }

        private Token TranseformUnableToParentingToken(Token token)
        {
            if (token.MarkToTranslate.CanBeParent) return token;
            token.ChildTokens.Clear();
            return token;
        }
    }
}