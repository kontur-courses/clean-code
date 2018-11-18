using System.Collections.Generic;


namespace Markdown
{
    public class Token
    {
        public readonly int StartIndex;
        public int EndIndex { get; private set; }
        public readonly Mark Mark;
        public List<Token> ChildTokens = new List<Token>();
        public readonly Token FatherToken;
        public Token(int startIndex, Mark mark, Token fatherToken = null)
        {
            StartIndex = startIndex;
            Mark = mark;
            FatherToken = fatherToken;
        }

        public void SetEndIndex(int endIndex)
        {
            EndIndex = endIndex;
        }
    }
}
