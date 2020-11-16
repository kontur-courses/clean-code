#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TextParser : ITextParser
    {
        public List<int> EscapingBackslashes { get; private set; }

        public List<Token> GetTokens(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(FindTokens(text));

            return tokens;
        }

        private IEnumerable<Token> FindTokens(string text)
        {
            EscapingBackslashes = FindEscapingBackslashes(text);
            var tokens = new List<Token>();

            var readers = new List<ITokenReader>()
            {
                new HeadingTokenReader(),
                new PlainTextTokenReader(),
                new StrongTokenReader(),
                new EmphasizedTokenReader(),
            };

            for (var i = 0; i < text.Length; ++i)
            {
                var token = readers
                    .Select(reader => reader.TryReadToken(this, text, i))
                    .FirstOrDefault(newToken => newToken != null);

                if (token != null)
                {
                    i = token.EndPosition;
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        private static List<int> FindEscapingBackslashes(string text)
        {
            var positions = new List<int>();

            for (var i = 0; i < text.Length; ++i)
            {
                if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\\')
                {
                    i++;
                }
                else if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] != '\\')
                {
                    positions.Add(i);
                }
            }

            return positions;
        }
    }
}