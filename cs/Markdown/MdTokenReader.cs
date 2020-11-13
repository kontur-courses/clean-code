using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdTokenReader
    {
        public readonly string Text;
        public int CurrentPosition = 0;

        public MdTokenReader(string text)
        {
            Text = text;
        }

        private MdToken nextToken;//Нужен, что-бы повторно не читать токен после SkipToNextToken
        public bool TryReadToken(out MdToken result, bool notRawText = false)
        {
            result = nextToken;
            nextToken = null;
            
            return result != null
                   || TryReadSpecializedTokens(out result)
                   || !notRawText && MdRawTextToken.TryRead(this, out result);
        }

        protected virtual bool TryReadSpecializedTokens(out MdToken result)
        {
            return MdHeaderToken.TryRead(this, out result)
                   || MdBoldToken.TryRead(this, out result)
                   || MdItalicToken.TryRead(this, out result);
        }

        public int SkipToNextToken(bool notRawText = true)
        {
            throw new NotImplementedException();
        }

        public string ReadAndParseAll()
        {
            throw new NotImplementedException();
        }

        public string ReadNextWord(bool inCurrentParagraph = true)
        {
            throw new NotImplementedException();
        }

        public string GetNextChars(int count)
        {
            throw new NotImplementedException();
        }

        public int SkipSpaces(bool inCurrentParagraph = true)
        {
            var position = GetNextNonSpaceCharPosition(inCurrentParagraph);
            var spacesCount = position - CurrentPosition;
            CurrentPosition = position;
            return spacesCount;
        }

        public int GetNextNonSpaceCharPosition(bool inCurrentParagraph = true)
        {
            throw new NotImplementedException();
        }
    }
}