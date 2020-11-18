using System;
using System.Collections.Generic;

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

        public bool TryReadToken(out Token result, Token parent = null, bool notRawText = false)
        {
            result = nextToken;
            nextToken = null;
            foreach (var readTokenFunc in Tokens.Values)
            {
                result ??= readTokenFunc(this, parent);
                if (result != null) return true;
            }

            return notRawText || TryReadRawTextUntil(out result, () => TryReadToken(out nextToken, parent, true));
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
            var position = CurrentPosition;
            var length = CountCharsUntil(stopWhen, failWhen);
            result = new MdRawTextToken(position, length >= 0 ? length : 0);
            return length >= 0;
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
                if (failWhen() || !TryReadToken(out var subtoken))
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

        public bool TryRead(string text)
        {
            if (GetNextChars(text.Length) != text) return false;
            CurrentPosition += text.Length;
            return true;
        }

        public bool IsWordBegin() => IsLineBegin() || Text[CurrentPosition - 1] == ' ';

        public bool IsWordEnd() => IsLineEnd() || Text[CurrentPosition + 1] == ' ';

        public bool IsLineBegin() => CurrentPosition == 0 || Text[CurrentPosition - 1] == '\n';

        public bool IsLineEnd() => CurrentPosition == Text.Length - 1 || Text[CurrentPosition + 1] == '\n';

        public string GetNextChars(int count)
        {
            if (count > Text.Length - CurrentPosition) count = Text.Length - CurrentPosition;
            return Text.Substring(CurrentPosition, count);
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