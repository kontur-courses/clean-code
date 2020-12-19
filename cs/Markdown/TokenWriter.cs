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

        public string Write(IToken token)
        {
            WriteToken(token);
            return newLine.ToString();
        }

        public void WriteToken(IToken token)
        {
            token.OpeningTag.ReplaceMdTagToHtmlTag(newLine, token);
            FillStringBeforeSubTokens(token);

            for (var i = 0; i < token.SubTokens.Count; i++)
            {
                WriteToken(token.SubTokens[i]);
                FillStringBetweenSubTokens(token, i);
            }

            FillStringAfterSubTokens(token);
            token.ClosingTag.ReplaceMdTagToHtmlTag(newLine, token);
        }

        private void FillStringBeforeSubTokens(IToken token)
        {
            if (token.SubTokens.Any())
                newLine.Append(inputLine.Substring(token.StartPosition + token.OpeningTag.MdTag.Length,
                    token.SubTokens[0].StartPosition - token.OpeningTag.MdTag.Length - token.StartPosition));
            // Здесь сложная обработка, потому что заголовок может заканчиваться как на \n, так и на конце всей строки
            else if (token.Type == TokenType.Header && token.EndPosition == inputLine.Length)
                newLine.Append(inputLine.Substring(token.StartPosition + token.OpeningTag.MdTag.Length,
                    token.Length - token.OpeningTag.MdTag.Length - token.ClosingTag.MdTag.Length + 1));
            else
                newLine.Append(inputLine.Substring(token.StartPosition + token.OpeningTag.MdTag.Length,
                    token.Length - token.OpeningTag.MdTag.Length - token.ClosingTag.MdTag.Length));
        }

        private void FillStringBetweenSubTokens(IToken token, int i)
        {
            if (i != token.SubTokens.Count - 1)
                newLine.Append(inputLine.Substring(token.SubTokens[i].EndPosition,
                    token.SubTokens[i + 1].StartPosition - token.SubTokens[i].EndPosition));
        }

        private void FillStringAfterSubTokens(IToken token)
        {
            if (token.SubTokens.Any())
            {
                var endOfLastSubToken = token.SubTokens.Last().EndPosition;
                if (endOfLastSubToken != inputLine.Length)
                    // То же самое, два типа окончания заголовка, один из которых нужно обрабатывать отдельно
                    if (token.Type == TokenType.Header && inputLine[token.EndPosition - 1] != '\n')
                        newLine.Append(inputLine.Substring(endOfLastSubToken,
                            token.EndPosition - endOfLastSubToken));
                    else
                        newLine.Append(inputLine.Substring(endOfLastSubToken,
                            token.EndPosition - token.ClosingTag.MdTag.Length - endOfLastSubToken));
            }
        }
    }
}
