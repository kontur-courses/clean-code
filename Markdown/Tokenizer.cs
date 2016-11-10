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
                yield return ReadNextToken(text, ref currentPosition);
            }
        }

        public Token ReadNextToken(string text, ref int startPosition)
        {
            throw new NotImplementedException();
        }

        public IShell ReadNextShell(string text, ref int startPosition, List<IShell> shells )
        {
            var currentPosition = startPosition;
            var prefix = new StringBuilder();
            IShell correctShell = null;
            while (currentPosition < text.Length)
            {
                prefix.Append(text[currentPosition]);
                if (shells.Any(s => s.GetPrefix().StartsWith(prefix.ToString())))
                {
                    var suitableShells = shells.Where(s => s.GetPrefix() == prefix.ToString()).ToList();
                    if (suitableShells.Any())
                    {
                        correctShell = suitableShells.First();
                    }
                    currentPosition++;
                }
                else
                {
                    break;
                }
            }
            if (currentPosition == text.Length || text[currentPosition] == ' ')
            {
                return null;
            }
            if (correctShell != null)
            {
                startPosition = currentPosition;
            }
            return correctShell;
        }

        public Tuple<int, IShell> GetEndPositionToken(string text, int currentPosition, List<IShell> shells, IShell currentShell)
        {
            var startOtherShell = -1;
            IShell otherShell = null;
            while (currentPosition < text.Length)
            {
                if (startOtherShell == -1)
                {
                    otherShell = ReadNextShell(text, ref currentPosition, shells);
                    if (otherShell != null)
                    {
                        startOtherShell = currentPosition - otherShell.GetPrefix().Length;
                        currentPosition--;
                        if (currentShell == null)
                        {
                            break;
                        }
                    }
                }
                currentPosition++;
            }
            return Tuple.Create(startOtherShell - 1, currentShell);
        }
    }
}
