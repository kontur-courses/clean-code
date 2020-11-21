using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TextParser : ITextParser
    {
        private readonly List<ITokenReader> readers = new List<ITokenReader>()
        {
            new HeadingTokenReader(),
            new StrongTokenReader(),
            new EmphasizedTokenReader(),
            new ImageTokenReader(),
            new PlainTextTokenReader()
        };

        public void AddTokenReaders(params ITokenReader[] tokenReaders)
        {
            readers.AddRange(tokenReaders);
        }

        public List<Token> GetTokens(string text, string context)
        {
            var tokens = new List<Token>();

            for (var i = 0; i < text.Length; ++i)
            {
                foreach (var reader in readers)
                {
                    if (!reader.TryReadToken(text, context, i, out var token))
                        continue;

                    if (token!.Value != text)
                    {
                        var newContext = text[token.Position..(token.EndPosition + 1)];
                        var childTokens = GetTokens(token!.Value, newContext);
                        token!.ChildTokens.AddRange(childTokens);
                    }

                    token.Value = RemoveBackslashes(token.Value);
                    tokens.Add(token);
                    i = token.EndPosition;
                    break;
                }
            }

            return tokens;
        }

        private string RemoveBackslashes(string text)
        {
            var newValue = new StringBuilder();
            var escapingBackslashes = FindBackslashes(text);

            for (var i = 0; i < text.Length; i++)
            {
                if (!escapingBackslashes.Contains(i))
                    newValue.Append(text[i]);
            }

            return newValue.ToString();
        }

        private List<int> FindBackslashes(string text)
        {
            var positions = new List<int>();

            for (var i = 0; i < text.Length; ++i)
                if (IsEscapingBackslash(text, i) && !positions.Contains(i - 1))
                    positions.Add(i);

            return positions;
        }

        private static bool IsEscapingBackslash(string text, int index)
        {
            return text[index] == '\\'
                   && (index + 1 == text.Length || !char.IsLetterOrDigit(text[index + 1]));
        }
    }
}