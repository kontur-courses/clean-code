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

        public Token ReadNextToken(string text, ref int currentPosition, List<IShell> shells)
        {
            var startPosition = currentPosition;
            var shell = ReadNextShell(text, ref currentPosition, shells);
            var left = currentPosition;
            var right = GetEndPositionToken(text, currentPosition, shells, shell);
            if (!IsRestrictedShell(text, shell, right))
            {
                currentPosition = startPosition;
                return ReadNextToken(text, ref currentPosition, shells.Where(s => !s.Equals(shell)).ToList());
            }
            currentPosition = right + 1 + (shell?.GetSuffix().Length ?? 0);
            return new Token(text.Substring(left, right - left + 1), shell);
        }

        private bool IsRestrictedShell(string text, IShell shell, int endToken)
        {
            return shell == null || IsSubstring(text, endToken + 1, shell.GetSuffix());
        }

        public IShell ReadNextShell(string text, ref int startPosition, List<IShell> shells )
        {
            var currentPosition = startPosition;
            if (currentPosition - 1 >= 0 && currentPosition < text.Length && text[currentPosition - 1] == '\\')
            {
                return null;
            }
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

        private bool IsSubstring(string text, int start, string substring)
        {
            if (start + substring.Length > text.Length)
            {
                return false;
            }
            return !substring.Where((t, i) => text[start + i] != t).Any();
        }

        public int GetEndPositionToken(string text, int currentPosition, List<IShell> shells, IShell currentShell)
        {
            for (; currentPosition < text.Length; currentPosition++)
            {
                if (currentShell == null)
                {
                    if (shells.Any(s => IsSubstring(text, currentPosition, s.GetPrefix())))
                    {
                        break;
                    }
                }
                else
                {
                    if (IsSubstring(text, currentPosition, currentShell.GetPrefix()))
                    {
                        if (currentPosition - 1 < 0 || text[currentPosition - 1] != ' ')
                        {
                            break;
                        }
                        
                    }
                }
            }
            return currentPosition - 1;
        }
    }
}
