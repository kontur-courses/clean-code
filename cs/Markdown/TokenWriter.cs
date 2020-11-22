using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class TokenWriter
    {
        private readonly string inputLine;
        private readonly StringBuilder newLine;

        public TokenWriter(string inputLine)
        {
            this.inputLine = inputLine;
            newLine = new StringBuilder();
        }

        public void Write(IToken token)
        {
            token.OpenTag(newLine, inputLine);


            for (var i = 0; i < token.SubTokens.Count; i++)
            {
                Write(token.SubTokens[i]);
                FillStringBetweenSubTokens(token, i);
            }

            token.CloseTag(newLine, inputLine);
        }

        private void FillStringBetweenSubTokens(IToken token, int i)
        {
            if (i != token.SubTokens.Count - 1)
                newLine.Append(inputLine.Substring(token.SubTokens[i].EndPosition,
                    token.SubTokens[i + 1].StartPosition - token.SubTokens[i].EndPosition));
        }

        public string GetString() => newLine.ToString();
    }
}
