using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Shell;

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
            if (!HasMoreTokens())
            {
                throw new InvalidOperationException();
            }
            var startPosition = currentPosition;
            var shell = ReadNextShell();
            var leftBorder = currentPosition;
            var rightBorder = GetEndPositionToken(shell);
            if (!shell?.IsRestricted(text, rightBorder + 1) ?? true)
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
            if (text.IsEscapedCharacter(positionAfterShell))
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
            if (text.IsIncorrectEndingShell(positionAfterShell) || correctShell == null)
            {
                return null;
            }
            if (text.IsSurroundedByNumbers(currentPosition, positionAfterShell - 1))
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
                    var newShell = shells.FirstOrDefault(s => s.GetPrefix().IsSubstring(text, endPositionToken));
                    if (newShell == null)
                    {
                        continue;
                    }
                    if (!text.IsSurroundedByNumbers(endPositionToken,
                            newShell.GetSuffix().GetPositionEndSubstring(endPositionToken)))
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
                    if (!currentShell.GetPrefix().IsSubstring(text, endPositionToken))
                    {
                        continue;
                    }
                    if (!text.IsSurroundedByNumbers(endPositionToken,
                            currentShell.GetSuffix().GetPositionEndSubstring(endPositionToken)))
                    {
                        break;
                    }
                }
            }
            return endPositionToken - 1;
        }

        private static IShell GetShellWithPrefix(IEnumerable<IShell> shells, string prefix)
        {
            return shells.FirstOrDefault(s => s.GetPrefix() == prefix);
        }
    }
}