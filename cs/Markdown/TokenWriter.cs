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

        public string Write(Token token)
        {
            WriteToken(token);
            return newLine.ToString();
        }

        public void WriteToken(Token token)
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

        private void FillStringBeforeSubTokens(Token token)
        {
            var endPosition = token.SubTokens.Any() 
                ? token.SubTokens[0].StartPosition
                : (token.Type == TokenType.Header && token.EndPosition == inputLine.Length)
                    ? token.EndPosition
                    : token.EndPosition - token.ClosingTag.MdTag.Length;
            var startPosition = token.StartPosition + token.OpeningTag.MdTag.Length;
            AppendLineExceptEscapedChars(startPosition, endPosition, token);
        }

        private void FillStringBetweenSubTokens(Token token, int i)
        {
            if (i != token.SubTokens.Count - 1)
            {
                var startPosition = token.SubTokens[i].EndPosition;
                var endPosition = token.SubTokens[i + 1].StartPosition;
                AppendLineExceptEscapedChars(startPosition, endPosition, token);
            }
        }

        private void FillStringAfterSubTokens(Token token)
        {
            if (token.SubTokens.Any())
            {
                var startPosition = token.SubTokens.Last().EndPosition;
                var endPosition = (token.Type == TokenType.Header && inputLine[token.EndPosition - 1] != '\n')
                    ? token.EndPosition
                    : token.EndPosition - token.ClosingTag.MdTag.Length;
                AppendLineExceptEscapedChars(startPosition, endPosition, token);
            }
        }

        private void AppendLineExceptEscapedChars(int startPosition, int endPosition, Token token)
        {
            for (var i = startPosition; i < endPosition; i++)
                if (!token.EscapedCharsPos.Contains(i))
                    newLine.Append(inputLine[i]);
        }
    }
}
