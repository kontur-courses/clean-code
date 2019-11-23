using System;
using System.Collections.Generic;

namespace Markdown.BasicTextTokenizer
{
    public static class TokenReaderExtensions
    {
        public static List<Token> ReadUntilWithEscapeProcessing(
            this TokenReader reader,
            Func<string, int, bool> isStopPosition, 
            Func<string, int, bool> isEscapePosition)
        {
            bool IsStopPositionWithEscape(string text, int position) => isEscapePosition(text, position)
                                                                        || isStopPosition(text, position);
            var tokens = new List<Token>();
            var firstTime = true;
            do
            {
                Token escapedToken = null;
                if (!firstTime)
                {
                    reader.SkipCount(1);
                    escapedToken = reader.ReadCount(1);
                }
                var token = reader.ReadUntil(IsStopPositionWithEscape);
                if (escapedToken != null)
                    token = escapedToken.Concat(token);
                if (token.Length != 0)
                    tokens.Add(token);
                firstTime = false;
            } while (isEscapePosition(reader.Text, reader.Position));
            return tokens;
        }
    }
}
