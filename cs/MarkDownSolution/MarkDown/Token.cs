using System.Collections.Generic;

namespace MarkDown
{
    public class Token
    {
        //bool closed { get; set; }
        int start { get; set; }
        int length { get; set; }

        public readonly List<Token> nestedTokens = new List<Token>();
        public Token(int start, int length)
        {
            this.start = start;
            this.length = length;
            //closed = true;
        }
        //public Token(int start)
        //{
        //    this.start = start;
        //    closed = false;
        //}
        //public void SetLength(int length)
        //{
        //    this.length = length;
        //    closed = true;
        //}
    }
}
