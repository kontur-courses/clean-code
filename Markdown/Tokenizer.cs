using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var currentPosition = startPosition;
            var prefix = new StringBuilder();
            IShell rightShell = null;
            while (currentPosition < text.Length)
            {
                prefix.Append(text[currentPosition]);
                if (shells.Any(s => s.GetPrefix().StartsWith(prefix.ToString())))
                {
                    var suitableShells = shells.Where(s => s.GetPrefix() == prefix.ToString());
                    if (suitableShells.Any())
                    {
                        rightShell = suitableShells.First();
                    }
                    currentPosition++;
                }
                else
                {
                    break;
                }
                
            }
            if (rightShell != null)
            {
                startPosition = currentPosition;
            }
            return rightShell;
        }

        public int GetEndPositionTextToken(string text, ref int startPosition, IShell currenShell)
        {
            throw new NotImplementedException();
        }
    }
}
