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
            var leftBorder = currentPosition;
            var rightBorder = GetEndPositionToken(text, currentPosition, shells, shell);
            if (!IsRestrictedShell(text, shell, rightBorder))
            {
                currentPosition = startPosition;
                leftBorder = startPosition;
                shell = null;
                rightBorder = GetEndPositionToken(text, currentPosition, shells, null);
            }
            currentPosition = rightBorder + 1 + (shell?.GetSuffix().Length ?? 0);
            return new Token(text.Substring(leftBorder, rightBorder - leftBorder + 1), shell);
        }

        public IShell ReadNextShell(string text, ref int startPosition, List<IShell> shells)
        {
            var currentPosition = startPosition;
            if (PreviousSymbolIsShielding(text, currentPosition))
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
                    correctShell = GetShellWithPrefix(shells, prefix.ToString());
                    currentPosition++;
                }
                else
                {
                    break;
                }
            }
            if (IsIncorrectEndingShell(text, currentPosition) || correctShell == null)
            {
                return null;
            }
            if (IsSurroundedByNumbers(text, startPosition, currentPosition - 1))
            {
                return null;
            }
            startPosition = currentPosition;
            return correctShell;
        }

        public int GetEndPositionToken(string text, int currentPosition, List<IShell> shells, IShell currentShell)
        {
            for (currentPosition++; currentPosition < text.Length; currentPosition++)
            {
                if (currentShell == null)
                {
                    var newShell = shells.FirstOrDefault(s => IsSubstring(text, currentPosition, s.GetPrefix()));
                    if (newShell == null) continue;
                    if (!IsSurroundedByNumbers(text, currentPosition, currentPosition - 1 + newShell.GetSuffix().Length))
                        break;
                }
                else
                {
                    if (text[currentPosition - 1] == '\\' || text[currentPosition - 1] == ' ') continue;
                    if (!IsSubstring(text, currentPosition, currentShell.GetPrefix())) continue;
                    if (!IsSurroundedByNumbers(text, currentPosition,
                            GetPositionEndText(currentPosition, currentShell.GetSuffix())))
                    {
                        break;
                    }
                }
            }
            return currentPosition - 1;
        }

        private static bool IsSurroundedByNumbers(string text, int startPrefix, int endSuffix)
        {
            int temp;
            return startPrefix > 0 && int.TryParse(text[startPrefix - 1].ToString(), out temp) &&
                   endSuffix + 1 < text.Length && int.TryParse(text[endSuffix + 1].ToString(), out temp);
        }

        private static int GetPositionEndText(int startPosition, string text)
        {
            return startPosition + text.Length - 1;
        }

        private static bool IsRestrictedShell(string text, IShell shell, int endToken)
        {
            return shell == null || IsSubstring(text, endToken + 1, shell.GetSuffix());
        }

        private static bool PreviousSymbolIsShielding(string text, int currentPosition)
        {
            return currentPosition - 1 >= 0 && currentPosition < text.Length && text[currentPosition - 1] == '\\';
        }

        private static bool IsIncorrectEndingShell(string text, int currentPosition)
        {
            return currentPosition >= text.Length || text[currentPosition] == ' ';
        }

        private static IShell GetShellWithPrefix(IEnumerable<IShell> shells, string prefix)
        {
            return shells.FirstOrDefault(s => s.GetPrefix() == prefix);
        }

        private static bool IsSubstring(string text, int start, string substring)
        {
            if (start + substring.Length > text.Length)
            {
                return false;
            }
            return !substring.Where((t, i) => text[start + i] != t).Any();
        }
    }
}