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

        public bool TryReadToken(out Token result, Token parent = null, bool notRawText = false, Func<bool> stopWhen = null)
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
            });
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
                if (failWhen() || !TryReadToken(out var subtoken, stopWhen: stopWhen))
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

        public bool IsAfterSpace() => IsLineBegin() || Text[CurrentPosition - 1] == ' ';

        public bool IsAtSpace() => IsLineEnd() || Text[CurrentPosition] == ' ';

        public bool IsLineBegin() => CurrentPosition == 0 || Text[CurrentPosition - 1] == '\n';

        public bool IsLineEnd() => CurrentPosition == Text.Length || Text[CurrentPosition] == '\n';

        public string GetNextChars(int count)
        {
            if (count > Text.Length - CurrentPosition) count = Text.Length - CurrentPosition;
            return Text.Substring(CurrentPosition, count);
        }

        public void AddBasicToken<TToken>(string startWith, string endWith, bool withOrWithoutSpaces = true) where TToken : TokenWithSubTokens, new()
        {
            if (!withOrWithoutSpaces)
                AddToken((reader, parent) => ReadBasicToken<TToken>(reader, parent, startWith, endWith));
            else
                AddToken((reader, parent)
                    => ReadBasicToken<TToken>(reader, parent, " " + startWith, endWith + " ")
                       ?? ReadBasicToken<TToken>(reader, parent, startWith, endWith, false));
        }

        public static TToken ReadBasicToken<TToken>(
            TokenReader reader, Token parent,
            string startWith, string endWith,
            bool allowSpaces = true)
            where TToken : TokenWithSubTokens, new()
        {
            var startWithNewLine = startWith.StartsWith("\n");
            var endWithNewLine = endWith.EndsWith("\n");
            
            var startWithNewWord = startWith.StartsWith(" ");
            var endWithNewWord = endWith.EndsWith(" ");
            
            if (startWithNewLine || startWithNewWord) startWith = startWith.Substring(1);
            if (endWithNewLine || endWithNewWord) endWith = endWith.Substring(0, endWith.Length - 1);
            
            var token = new TToken();
            token.StartPosition = reader.CurrentPosition;
            token.Length += startWith.Length;
            token.Parent = parent;

            var state = reader.GetCurrentState();

            var ok = (!startWithNewLine || reader.IsLineBegin())
                     && (!startWithNewWord || reader.IsAfterSpace())

                     && reader.TryRead(startWith)
                     && !reader.IsAtSpace()
                     
                     && reader.TryReadSubtokensUntil(token,
                         () => reader.TryGet(endWith, endWithNewLine),
                         () => !allowSpaces && reader.IsAtSpace())
                     
                     && !reader.IsAfterSpace()
                     && reader.TryRead(endWith)

                     && (!endWithNewWord || reader.IsAtSpace())
                     && (!endWithNewLine || reader.IsLineEnd());

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