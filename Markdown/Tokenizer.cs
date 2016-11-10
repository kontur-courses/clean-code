using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tokenizer
    {
        public IEnumerable<Token> SplitToTokens(string text, List<IShell> shells)
        {
            var currentPosition = 0;
            while (currentPosition < text.Length)
            {
                yield return ReadNextToken(text, ref currentPosition, shells);
            }
        }

        public Token ReadNextToken(string text, ref int startPosition, List<IShell> shells)
        {            
            var shell = ReadNextShell(text, ref startPosition, shells);
            var left = startPosition;
            var right = GetEndPositionToken(text, startPosition, shells, shell);
            startPosition = right + 1 + (shell?.GetSuffix().Length ?? 0);
            return new Token(text.Substring(left, right - left + 1), shell);
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
            if (currentPosition >= text.Length || text[currentPosition] == ' ')
            {
                return null;
            }
            if (correctShell != null)
            {
                startPosition = currentPosition;
            }
            return correctShell;
        }

        private bool isSubstring(string text, int start, string substring)
        {
            for (var i = 0; i < substring.Length; i++)
            {
                if (text[start + i] != substring[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetEndPositionToken(string text, int currentPosition, List<IShell> shells, IShell currentShell)
        {
            for (; currentPosition < text.Length; currentPosition++)
            {
                if (currentShell == null)
                {
                    if (shells.Any(s => isSubstring(text, currentPosition, s.GetPrefix())))
                    {
                        break;
                    }
                }
                else
                {
                    if (isSubstring(text, currentPosition, currentShell.GetPrefix()))
                    {
                        break;
                    }
                }
            }
            return currentPosition - 1;
        }
    }
}
