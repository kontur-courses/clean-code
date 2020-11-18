using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class TokenReader
    {
        public readonly string Text;
        public int CurrentPosition = 0;

        private readonly Dictionary<Type, Func<TokenReader, Token, Token>> Tokens =
            new Dictionary<Type, Func<TokenReader, Token, Token>>();

        public TokenReader(string text)
        {
            Text = text;
        }

        private Token nextToken;

        public bool TryReadToken(out Token result, Token parent = null, bool notRawText = false,
            Func<bool> stopWhen = null, Func<bool> failWhen = null)
        {
            result = nextToken;
            nextToken = null;
            if (result != null)
            {
                CurrentPosition += result.Length;
                return true;
            }
            
            foreach (var readTokenFunc in Tokens.Values)
            {
                result = readTokenFunc(this, parent);
                if (result != null) return true;
            }

            if (notRawText || CurrentPosition == Text.Length) return false;
            return TryReadRawTextUntil(out result, () =>
            {
                if (stopWhen != null && stopWhen()) return true;
                var state = GetCurrentState();
                var ok = TryReadToken(out nextToken, parent, true);
                state.Undo();
                return ok;
            }, failWhen ?? (() => false));
        }

        public void AddToken<TToken>(Func<TokenReader, Token, TToken> readTokenFunc) where TToken : Token
            => Tokens[typeof(TToken)] = readTokenFunc;

        public void RemoveToken<TToken>() where TToken : Token
            => Tokens.Remove(typeof(TToken));

        public bool TryRead<TToken>(out Token result, Token parent) where TToken : Token
        {
            result = null;
            if (!Tokens.TryGetValue(typeof(TToken), out var readFunc)) return false;
            return (result = readFunc(this, parent)) != null;
        }

        public bool TryReadRawTextUntil(out Token result, Func<bool> stopWhen)
            => TryReadRawTextUntil(out result, stopWhen, () => false);

        public bool TryReadRawTextUntil(out Token result, Func<bool> stopWhen, Func<bool> failWhen)
        {
            var state = GetCurrentState();
            var initialPosition = CurrentPosition;
            for (; CurrentPosition < Text.Length && !stopWhen(); CurrentPosition++)
            {
                if (!failWhen()) continue;
                state.Undo();
                result = null;
                return false;
            }
            
            result = new MdRawTextToken(initialPosition, CurrentPosition - initialPosition);
            return true;
        }

        public IEnumerable<Token> ReadAll()
        {
            while (TryReadToken(out var token)) yield return token;
        }


        public bool TryReadSubtokensUntil(TokenWithSubTokens output, Func<bool> stopWhen)
            => TryReadSubtokensUntil(output, stopWhen, () => false);

        public bool TryReadSubtokensUntil(TokenWithSubTokens output, Func<bool> stopWhen, Func<bool> failWhen)
        {
            var initialCount = output.GetSubtokenCount();
            while (!stopWhen())
            {
                if (failWhen() || !TryReadToken(out var subtoken, output, stopWhen: stopWhen, failWhen: failWhen))
                {
                    output.SetSubtokenCount(initialCount);
                    return false;
                }

                output.AddSubtoken(subtoken);
            }

            return true;
        }

        public bool SkipUntil(Func<bool> stopWhen) => SkipUntil(stopWhen, () => false);

        public bool SkipUntil(Func<bool> stopWhen, Func<bool> failWhen)
        {
            var count = CountCharsUntil(stopWhen, failWhen);
            CurrentPosition += count >= 0 ? count : 0;
            return count >= 0;
        }

        public int CountCharsUntil(Func<bool> stopWhen) => CountCharsUntil(stopWhen, () => false);

        public int CountCharsUntil(Func<bool> stopWhen, Func<bool> failWhen)
        {
            var position = CurrentPosition;
            var initialPosition = position;
            for (; position < Text.Length && !stopWhen(); position++)
                if (failWhen())
                    return -1;

            return position - initialPosition;
        }

        public bool TryRead(string text, bool endWithNewLine = false)
        {
            if (!TryGet(text, endWithNewLine)) return false;
            CurrentPosition += text.Length;
            return true;
        }

        public bool TryGet(string text, bool endWithNewLine = false)
            => GetNextChars(text.Length) == text && (!endWithNewLine || IsLineEnd());

        public bool IsAfterSpace(int offset = 0) => IsLineBegin(offset) || Text[CurrentPosition - 1 + offset] == ' ';

        public bool IsAtSpace(int offset = 0) => IsLineEnd(offset) || Text[CurrentPosition + offset] == ' ';

        public bool IsLineBegin(int offset = 0)
            => CurrentPosition + offset <= 0 || Text[CurrentPosition - 1 + offset] == '\n';

        public bool IsLineEnd(int offset = 0)
            => CurrentPosition + offset >= Text.Length || Text[CurrentPosition + offset] == '\n';

        public string GetNextChars(int count)
        {
            if (count > Text.Length - CurrentPosition) count = Text.Length - CurrentPosition;
            return Text.Substring(CurrentPosition, count);
        }

        public void AddBasicToken<TToken>(string startWith, string endWith, params Type[] disallowedParrentTokens)
            where TToken : TokenWithSubTokens, new()
        {
            AddToken((reader, parent) => (parent != null && parent.IsInsideAnyTokenOfType(disallowedParrentTokens))
                ? null
                : ReadBasicToken<TToken>(reader, parent, startWith, endWith));
        }

        public static TToken ReadBasicToken<TToken>(
            TokenReader reader, Token parent,
            string startWith, string endWith)
            where TToken : TokenWithSubTokens, new()
        {
            if (parent != null && parent.IsInsideAnyTokenOfType(typeof(TToken))) return null;
            var shouldStartWithNewLine = startWith.StartsWith("\n");
            var shouldEndWithNewLine = endWith.EndsWith("\n");

            var startWithNewWord = reader.IsAfterSpace();
            
            if (shouldStartWithNewLine) startWith = startWith.Substring(1);
            if (shouldEndWithNewLine) endWith = endWith.Substring(0, endWith.Length - 1);

            var token = new TToken {StartPosition = reader.CurrentPosition, Length = startWith.Length, Parent = parent};

            var state = reader.GetCurrentState();
            var wasSpaces = false;

            var ok = (!shouldStartWithNewLine || reader.IsLineBegin())

                     && reader.TryRead(startWith)
                     && !reader.IsAtSpace()

                     && reader.TryReadSubtokensUntil(token,
                         () => !reader.IsAfterSpace()
                               && (!wasSpaces || reader.IsAtSpace(endWith.Length))
                               && reader.TryGet(endWith, shouldEndWithNewLine),
                         () => (wasSpaces |= reader.IsAtSpace()) && !startWithNewWord)

                     && !reader.IsAfterSpace()
                     && reader.TryRead(endWith)

                     && (!wasSpaces || reader.IsAtSpace())
                     && (!shouldEndWithNewLine || reader.IsLineEnd());

            token.Length += endWith.Length;
            if (!ok || token.Length == startWith.Length + endWith.Length)
            {
                state.Undo();
                return null;
            }

            return token;
        }

        public virtual TokenReaderState GetCurrentState() => new TokenReaderState(this);

        public class TokenReaderState
        {
            public readonly TokenReader Reader;

            private int position;

            public TokenReaderState(TokenReader reader)
            {
                Reader = reader;
                position = reader.CurrentPosition;
            }

            public bool Undo()
            {
                UndoAction();
                return false; //всегда false что-бы было легко использовать в выражениях с TryRead
            }

            protected virtual void UndoAction()
            {
                Reader.CurrentPosition = position;
            }
        }
    }
}