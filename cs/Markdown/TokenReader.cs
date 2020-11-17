using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class TokenReader
    {
        protected readonly string Text;
        protected int CurrentPosition = 0;

        public TokenReader(string text)
        {
            Text = text;
        }

        private MdToken nextToken;
        public bool TryReadToken(out MdToken result, bool notRawText = false)
        {
            result = nextToken;
            nextToken = null;
            return result != null
                   || TryReadSpecifiedTokens(out result)
                   || !notRawText && TryRead(new MdRawTextToken(CurrentPosition), out result,
                       () => TryReadSpecifiedTokens(out nextToken));
        }

        protected abstract bool TryReadSpecifiedTokens(out MdToken result);

        protected bool TryRead(MdRawTextToken token, out MdToken result)
            => TryRead(token, out result, () => false);

        protected virtual bool TryRead(MdRawTextToken token, out MdToken result, Func<bool> stopWhen)
            => throw new NotImplementedException();

        public IEnumerable<MdToken> ReadAll() => throw new NotImplementedException();


        protected bool TryReadSubtokensUntil(MdTokenWithSubTokens output, Func<bool> stopWhen)
            => TryReadSubtokensUntil(output, stopWhen, () => false);

        protected bool TryReadSubtokensUntil(MdTokenWithSubTokens output, Func<bool> stopWhen, Func<bool> failWhen)
            => throw new NotImplementedException();

        protected bool TryRead(string text)
        {
            if (GetNextChars(text.Length) != text) return false;
            CurrentPosition += text.Length;
            return true;
        }

        protected bool IsWordBegin() => IsLineBegining() || Text[CurrentPosition - 1] == ' ';

        protected bool IsWordEnd() => IsLineEnd() || Text[CurrentPosition + 1] == ' ';

        protected bool IsLineBegining() => CurrentPosition == 0 || Text[CurrentPosition - 1] == '\n';

        protected bool IsLineEnd() => CurrentPosition == Text.Length - 1 || Text[CurrentPosition + 1] == '\n';

        public string GetNextChars(int count)
        {
            if (count > Text.Length - CurrentPosition) count = Text.Length - CurrentPosition;
            return Text.Substring(CurrentPosition, count);
        }

        protected virtual TokenReaderState GetCurrentState() => new TokenReaderState(this);

        protected class TokenReaderState
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

            protected virtual void UndoAction() => Reader.CurrentPosition = position;
        }
    }
}