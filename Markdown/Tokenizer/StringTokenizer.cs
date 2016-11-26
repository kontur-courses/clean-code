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

        public IEnumerable<Token> ReadAllTokens()
        {
            while (HasMoreTokens())
            {
                yield return NextToken();
            }
        }


        public bool HasMoreTokens()
        {
            return currentPosition < text.Length;
        }

        public Token NextToken()
        {
            if (!HasMoreTokens())
            {
                throw new InvalidOperationException("impossible to get the next token. all tokens listed");
            }
            var startPosition = currentPosition;
            var shell = ReadNextShell();
            if (shell == null)
            {
                return new Token(ReadRawText(), new List<Attribute>());
            }
            var shellSuffix = GetShellEnd(shell);
            if (shellSuffix != null)
            {
                var tokenText = text.Substring(currentPosition, shellSuffix.Start - currentPosition);
                currentPosition = shellSuffix.End + 1;
                return new Token(tokenText, shellSuffix.Attributes, shell);
            }
            currentPosition = startPosition;
            return new Token(ReadRawText(), new List<Attribute>());
        }

        private IShell ReadNextShell()
        {
            var maxPrefix = 0;
            IShell resultShell = null;
            var startPosition = currentPosition;
            foreach (var shell in shells)
            {
                MatchObject matchObject;
                if (shell.TryOpen(text, startPosition, out matchObject))
                {
                    if (matchObject.Length > maxPrefix)
                    {
                        resultShell = shell;
                        currentPosition = matchObject.End + 1;
                    }
                }
            }
            return resultShell;
        }

        private string ReadRawText()
        {
            var readPosition = currentPosition;
            var tokenText = new StringBuilder();
            for (; readPosition < text.Length; readPosition++)
            {
                tokenText.Append(text[readPosition]);
                MatchObject tempMatchObject;
                if (readPosition != text.Length - 1 &&
                    shells.Any(s => s.TryOpen(text, readPosition + 1, out tempMatchObject)))
                {
                    readPosition++;
                    break;
                }
            }
            currentPosition = readPosition;

            return tokenText.ToString();
        }

        private MatchObject GetShellEnd(IShell currentShell)
        {
            var readPosition = currentPosition;
            for (; readPosition < text.Length; readPosition++)
            {
                MatchObject matchObject;
                if (currentShell.TryClose(text, readPosition, out matchObject))
                {
                    return matchObject;
                }
            }
            return null;
        }
    }
}