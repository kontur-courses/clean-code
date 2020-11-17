using System;

namespace Markdown
{
    public class MdTokenReader : TokenReader
    {
        public MdTokenReader(string text) : base(text)
        {
        }

        protected override bool TryReadSpecifiedTokens(out MdToken result)
        {
            return TryRead(new MdHeaderToken(CurrentPosition), out result)
                   || TryRead(new MdBoldToken(CurrentPosition), out result)
                   || TryRead(new MdItalicToken(CurrentPosition), out result);
        }

        protected override bool TryRead(MdRawTextToken token, out MdToken result, Func<bool> stopWhen) =>
            throw new NotImplementedException();

        protected virtual bool TryRead(MdHeaderToken token, out MdToken result)
        {
            result = token;
            var state = GetCurrentState();
            return IsLineBegining()
                   && TryRead("# ")
                   && TryReadSubtokensUntil(token, IsLineEnd)
                   || state.Undo();
        }

        protected virtual bool TryRead(MdBoldToken token, out MdToken result) => throw new NotImplementedException();

        protected virtual bool TryRead(MdItalicToken token, out MdToken result) => throw new NotImplementedException();
    }
}