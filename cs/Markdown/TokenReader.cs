using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class TokenReader
    {
        public readonly string Text;
        public int CurrentPosition = 0;

        private readonly Dictionary<Type, Func<TokenReader, MdToken, MdToken>> Tokens =
            new Dictionary<Type, Func<TokenReader, MdToken, MdToken>>();

        public TokenReader(string text)
        {
            Text = text;
        }

        private MdToken nextToken;

        public bool TryReadToken(out MdToken result, MdToken parent = null, bool notRawText = false)
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

        public void AddToken<TToken>(Func<TokenReader, MdToken, TToken> readTokenFunc) where TToken : MdToken
            => Tokens[typeof(TToken)] = readTokenFunc;

        public void RemoveToken<TToken>() where TToken : MdToken
            => Tokens.Remove(typeof(TToken));

        public bool TryRead<TToken>(out MdToken result, MdToken parent) where TToken : MdToken
        {
            result = null;
            if (!Tokens.TryGetValue(typeof(TToken), out var readFunc)) return false;
            return (result = readFunc(this, parent)) != null;
        }

        public bool TryReadRawTextUntil(out MdToken result, Func<bool> stopWhen)
            => TryReadRawTextUntil(out result, stopWhen, () => false);

        public bool TryReadRawTextUntil(out MdToken result, Func<bool> stopWhen, Func<bool> failWhen)
            => throw new NotImplementedException();

        public IEnumerable<MdToken> ReadAll() => throw new NotImplementedException();


        public bool TryReadSubtokensUntil(MdTokenWithSubTokens output, Func<bool> stopWhen)
            => TryReadSubtokensUntil(output, stopWhen, () => false);

        public bool TryReadSubtokensUntil(MdTokenWithSubTokens output, Func<bool> stopWhen, Func<bool> failWhen)
            => throw new NotImplementedException();

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