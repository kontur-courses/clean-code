using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Markdown
{
    public class TokenReader
    {
        public int CurrentPosition { get; private set; }
        private List<(int, LexType)> Tokens;
        public TokenReader(List<(int, LexType)> tokens)
        {
            Tokens = tokens;
            CurrentPosition = 0;
        }

        public void Next()
        {
            CurrentPosition++;
        }

        public (int, LexType) Current()
        {
            return CurrentPosition >= Tokens.Count ? default : Tokens[CurrentPosition];
        }

        public int CurrentValue()
        {
            return CurrentPosition >= Tokens.Count ? default : Tokens[CurrentPosition].Item1;
        }

        public (int, LexType) Previous()
        {
            return CurrentPosition == 0 ? default : Tokens[CurrentPosition - 1];
        }

        public (int, LexType) PeekNext()
        {
            return CurrentPosition == Tokens.Count - 1 ? default : Tokens[CurrentPosition + 1];
        }

        public bool EndReached()
        {
            return CurrentPosition >= Tokens.Count;
        }
    }
}
