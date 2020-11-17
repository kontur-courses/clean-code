using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TextParser : ITextParser
    {
        private List<int> EscapingBackslashes { get; set; }
        private readonly List<ITokenReader> readers = new List<ITokenReader>()
        {
            new HeadingTokenReader(),
            new StrongTokenReader(),
            new EmphasizedTokenReader(),
            new PlainTextTokenReader()
        };

        public void AddTokenReaders(params ITokenReader[] tokenReaders)
        {
            readers.AddRange(tokenReaders);
        }

        public List<Token> GetTokens(string text)
        {
            var tokens = new List<Token>();
            EscapingBackslashes = this.FindEscapingBackslashes(text);

            for (var i = 0; i < text.Length; ++i)
            {
                var token = readers
                    .Select(reader => reader.TryReadToken(text, i))
                    .FirstOrDefault(newToken => newToken != null);

                if (token == null)
                    throw new ArgumentNullException();

                if (!IsNotDuplicatedToken(tokens, token))
                {
                    tokens.Add(token);
                }
            }

            tokens = ReplaceIntersectedTokens(tokens, text);

            foreach (var token in tokens)
            {
                token.Value = RemoveEscapingBackslashes(token);
            }

            return tokens.OrderBy(x => x.Position)
                .Where(x => x.Value != "")
                .ToList();
        }

        private static bool IsNotDuplicatedToken(List<Token> tokens, Token token)
        {
            return tokens
                .Where(x => x != token)
                .Any(x => token.IsIntersecting(x)
                              && token.Type != TokenType.Emphasized
                              && token.Type != TokenType.Strong);
        }

        private static List<Token> ReplaceIntersectedTokens(List<Token> tokens, string text)
        {
            var checkedTokens = new List<Token>();

            foreach (var token in tokens)
            {
                var intersectedToken = tokens
                    .Where(x => x != token)
                    .FirstOrDefault(x => x.IsCollided(token));

                if (intersectedToken != null)
                {
                    var position = Math.Min(intersectedToken.Position, token.Position);
                    var endPosition = Math.Max(intersectedToken.EndPosition, token.EndPosition);
                    var value = text[position..(endPosition + 1)];

                    if (checkedTokens.All(x => x.Value != value))
                        checkedTokens.Add(new Token(position, value, TokenType.PlainText));

                    continue;
                }

                checkedTokens.Add(token);
            }

            return checkedTokens;
        }

        private string RemoveEscapingBackslashes(Token token)
        {
            var result = new StringBuilder();

            for (var i = 0; i < token.Value.Length; ++i)
                if (!EscapingBackslashes.Contains(token.Position + i))
                    result.Append(token.Value[i]);

            return result.ToString();
        }
    }
}