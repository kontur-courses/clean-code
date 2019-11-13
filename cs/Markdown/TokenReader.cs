using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class TokenReader
    {
        public readonly string StartLine;
        public readonly string StopLine;

        private Func<string, bool> ruleForTokenContent;

        public TokenReader(string startLine, string stopLine)
        {
            StartLine = startLine;
            StopLine = stopLine;
        }

        public void AddRuleForTokenContent(Func<string, bool> ruleForTokenContent)
        {
            this.ruleForTokenContent = ruleForTokenContent;
        }

        public List<Token> SplitToTokens(string text)
        {
            var tokens = new List<Token>();
            var lastTokenEndPosition = 0;
            for (var startLineIndex = text.IndexOf(StartLine, 0); startLineIndex != -1; startLineIndex = text.IndexOf(StartLine, lastTokenEndPosition))
            {
                var token = ReadToken(text, startLineIndex + StartLine.Length);
                var length = token.Position - lastTokenEndPosition;
                if (token.IsInterior)
                    length -= StartLine.Length;
                if (length > 0)
                    tokens.Add(new Token(lastTokenEndPosition,
                        text.Substring(lastTokenEndPosition, length), false));

                lastTokenEndPosition = token.Position + token.Length;
                if (token.IsInterior)
                    lastTokenEndPosition += StopLine.Length;
                tokens.Add(token);
            }
            if (lastTokenEndPosition < text.Length)
                tokens.Add(new Token(lastTokenEndPosition, text.Substring(lastTokenEndPosition), false));
            return tokens;
        }

        public Token ReadToken(string text, int position)
        {
            var stopLineIndex = text.IndexOf(StopLine, position);
            if (stopLineIndex == -1)
                throw new ArgumentException("Stop line not found");
            var tokenText = text.Substring(position, stopLineIndex - position);
            if (ruleForTokenContent(tokenText))
                return new Token(position, tokenText, true);
            return new Token(position - StartLine.Length, StartLine + tokenText + StopLine, false);
        }
    }
}
