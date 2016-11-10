using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        public IEnumerable<Token> SplitToTokens(string text, IEnumerable<IShell> shells)
        {
            var currentPosition = 0;
            while (currentPosition < text.Length)
            {
                yield return GetNextToken(text, ref currentPosition);
            }
        }

        public Token GetNextToken(string text, ref int startPosition)
        {
            throw new NotImplementedException();
        }

        public IShell GetNextShell(string text, ref int startPosition, IEnumerable<IShell> shells )
        {
            throw new NotImplementedException();
        }

        public int GetEndPositionTextToken(string text, ref int startPosition, IShell currenShell)
        {
            throw new NotImplementedException();
        }
    }
}
