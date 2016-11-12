using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Shells;

namespace Markdown.Tokenizer
{
    public class StringTokenizer
    {
        private readonly string text;
        private readonly List<IShell> shells;
        private int currentPosition;

        public StringTokenizer(string text, List<IShell> shells)
        {
            this.text = text;
            this.shells = shells;
        }


        public bool HasMoreTokens()
        {
            return currentPosition < text.Length;
        }

        public Token NextToken()
        {
            var startPosition = currentPosition;
            var shell = ReadNextShell();
            var leftBorder = currentPosition;
            var rightBorder = GetEndPositionToken(shell);
            if (!IsRestrictedShell(text, shell, rightBorder))
            {
                currentPosition = startPosition;
                leftBorder = startPosition;
                shell = null;
                rightBorder = GetEndPositionToken(null);
            }
            currentPosition = rightBorder + 1 + (shell?.GetSuffix().Length ?? 0);
            return new Token(text.Substring(leftBorder, rightBorder - leftBorder + 1), shell);
        }

        private IShell ReadNextShell()
        {
            var positionAfterShell = currentPosition;
            if (PreviousSymbolIsShielding(text, positionAfterShell))
            {
                return null;
            }
            var prefix = new StringBuilder();
            IShell correctShell = null;
            while (positionAfterShell < text.Length)
            {
                prefix.Append(text[positionAfterShell]);
                if (shells.Any(s => s.GetPrefix().StartsWith(prefix.ToString())))
                {
                    correctShell = GetShellWithPrefix(shells, prefix.ToString());
                    positionAfterShell++;
                }
                else
                {
                    break;
                }
            }
            if (IsIncorrectEndingShell(text, positionAfterShell) || correctShell == null)
            {
                return null;
            }
            if (IsSurroundedByNumbers(text, currentPosition, positionAfterShell - 1))
            {
                return null;
            }
            currentPosition = positionAfterShell;
            return correctShell;
        }

        private int GetEndPositionToken(IShell currentShell)
        {
            var endPositionToken = currentPosition;
            for (endPositionToken++; endPositionToken < text.Length; endPositionToken++)
            {
                if (currentShell == null)
                {
                    var newShell = shells.FirstOrDefault(s => IsSubstring(text, endPositionToken, s.GetPrefix()));
                    if (newShell == null)
                    {
                        continue;
                    }
                    if (!IsSurroundedByNumbers(text, endPositionToken,
                            GetPositionEndSubstring(endPositionToken, newShell.GetSuffix())))
                    {
                        break;
                    }
                }
                else
                {
                    if (text[endPositionToken - 1] == '\\' || text[endPositionToken - 1] == ' ')
                    {
                        continue;
                    }
                    if (!IsSubstring(text, endPositionToken, currentShell.GetPrefix()))
                    {
                        continue;
                    }
                    if (!IsSurroundedByNumbers(text, endPositionToken,
                            GetPositionEndSubstring(endPositionToken, currentShell.GetSuffix())))
                    {
                        break;
                    }
                }
            }
            return endPositionToken - 1;
        }

        private static bool IsSurroundedByNumbers(string text, int startPrefix, int endSuffix)
        {
            int temp;
            return startPrefix > 0 && int.TryParse(text[startPrefix - 1].ToString(), out temp) &&
                   endSuffix + 1 < text.Length && int.TryParse(text[endSuffix + 1].ToString(), out temp);
        }

        private static int GetPositionEndSubstring(int startPosition, string text)
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