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

        public string ReadAndParseAll()
        {
            throw new NotImplementedException();
        }

        public MdToken ReadToken()
        {
            throw new NotImplementedException();
        }

        public string ParseToken(MdToken token)
        {
            throw new NotImplementedException();
        }

        private int SkipUntilNextToken(MdTokenTypes currentTokenType = MdTokenTypes.RawText)
        {
            throw new NotImplementedException();
        }

        private string ReadNextWord(bool inCurrentParagraph = true)
        {
            throw new NotImplementedException();
        }

        private string GetNextChars(int count)
        {
            throw new NotImplementedException();
        }

        private int SkipSpaces(bool inCurrentParagraph = true)
        {
            var position = GetNextNonSpaceCharPosition(inCurrentParagraph);
            var spacesCount = position - CurrentPosition;
            CurrentPosition = position;
            return spacesCount;
        }

        private int GetNextNonSpaceCharPosition(bool inCurrentParagraph = true)
        {
            throw new NotImplementedException();
        }

        private static readonly Dictionary<MdTokenTypes, (string startTag, string endTag)> Tags = new Dictionary<MdTokenTypes, (string, string)>()
        {
            {MdTokenTypes.RawText, ("", "")},
            {MdTokenTypes.Italic, ("<em>", "</em>")},
            {MdTokenTypes.Bold, ("<strong>", "</strong>")},
            {MdTokenTypes.Header, ("<h1>", "</h1>")},
        };

        public enum MdTokenTypes
        {
            RawText,
            Italic,
            Bold,
            Header
        }

        public class MdToken
        {
            public int StartPosition;
            public int Length;
            public MdTokenTypes TokenType;
            public List<MdToken> SubTokens = new List<MdToken>();
        }
    }
}